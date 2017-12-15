#pragma once
#include <iostream>
#include <string>

#include <opencv2\core.hpp>

using namespace cv;
using namespace std;

#define __CL_ENABLE_EXCEPTIONS
#include <CL/cl.hpp>
namespace gpu
{
	void contratAndBirghtness_cuda(Mat& source, float alpha, int beta, cl::Device device)
	{
		static const char contrastAndBrightnessSource[] =
			"#if defined(cl_khr_fp64)\n"
			"#  pragma OPENCL EXTENSION cl_khr_fp64: enable\n"
			"#elif defined(cl_amd_fp64)\n"
			"#  pragma OPENCL EXTENSION cl_amd_fp64: enable\n"
			"#else\n"
			"#  error double precision is not supported\n"
			"#endif\n"
			"kernel void AddWeighted(\n"
			"       global const unsigned char *Src,\n"
			"       global unsigned char *Dst,\n"
			"       float alpha,\n"
			"       int beta\n"
			"       )\n"
			"{\n"
			"    size_t i = get_global_id(0);\n"
			"    float res = (float)Src[i]*alpha+beta;\n"
			"	 float sature = 1;"
			"		if(res > 255) sature = 255;"
			"		else if(res < 0) sature = 30;"
			"		else sature = res;"
			"		Dst[i] = sature;"
			"}\n";

		try
		{/*
			std::vector<cl::Platform> platform;
			cl::Platform::get(&platform);

			if (platform.empty())
			{
				throw;
			}

			cl::Context context;
			std::vector<cl::Device> device;
			for (auto p = platform.begin(); device.empty() && p != platform.end(); p++)
			{
				std::vector<cl::Device> pldev;

				try
				{
					p->getDevices(CL_DEVICE_TYPE_GPU, &pldev);

					for (auto d = pldev.begin(); device.empty() && d != pldev.end(); d++)
					{
						if (!d->getInfo<CL_DEVICE_AVAILABLE>()) continue;

						std::string ext = d->getInfo<CL_DEVICE_EXTENSIONS>();

						if (
							ext.find("cl_khr_fp64") == std::string::npos &&
							ext.find("cl_amd_fp64") == std::string::npos
							) continue;

						device.push_back(*d);
						context = cl::Context(device);
					}
				}
				catch (...)
				{
					device.clear();
				}
			}

			if (device.empty())
			{
				throw;
			}*/
			cout << "Device: " << device.getInfo<CL_DEVICE_NAME>() << endl;
			cl::Context context(device);
			cl::CommandQueue queue(context, device);

			cl::Program program(context, cl::Program::Sources(
				1, std::make_pair(contrastAndBrightnessSource, strlen(contrastAndBrightnessSource))
			));

			try
			{
				program.build(std::vector<cl::Device>{device});
			}
			catch (const cl::Error&)
			{
				std::cerr
					<< "OpenCL compilation error" << std::endl
					<< program.getBuildInfo<CL_PROGRAM_BUILD_LOG>(device)
					<< std::endl;
				throw;
			}

			cl::Kernel Floyd(program, "AddWeighted");

			// Allocate device buffers and transfer input data to device.

			Mat c = source.clone();
			cl::Buffer Src(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols*source.channels() * sizeof(uchar), source.data);

			cl::Buffer Dst(context, CL_MEM_WRITE_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols *source.channels() * sizeof(uchar), source.data);

			// Set kernel parameters.
			Floyd.setArg(0, Src);
			Floyd.setArg(1, Dst);
			Floyd.setArg(2, static_cast<float>(alpha));
			Floyd.setArg(3, static_cast<int>(beta));

			size_t N = source.rows*source.cols*source.channels();
			// Launch kernel on the compute device.
			queue.enqueueNDRangeKernel(Floyd, cl::NullRange, N, cl::NullRange);

			// Get result back to host.
			queue.enqueueReadBuffer(Dst, CL_TRUE, 0, source.rows*source.cols *source.channels() * sizeof(uchar), source.data);


		}
		catch (const cl::Error &err)
		{
			std::cerr
				<< "OpenCL error: "
				<< err.what() << "(" << err.err() << ")"
				<< std::endl;
			throw;
		}
	}
}