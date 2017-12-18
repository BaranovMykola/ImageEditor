#pragma once
#include <iostream>
#include <string>
#include <fstream>

#include <opencv2\core.hpp>

using namespace cv;
using namespace std;

#define __CL_ENABLE_EXCEPTIONS
#include <CL/cl.hpp>
namespace gpu
{
	void contratAndBirghtness_cuda(Mat& source, float alpha, int beta, cl::Device device)
	{
		ifstream f("contrastAndBrightness.cl");
		string str((std::istreambuf_iterator<char>(f)),
				   std::istreambuf_iterator<char>());
		const char* contrastAndBrightnessSource = str.c_str();

		try
		{
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

			cl::Kernel ContrastKernel(program, "AddWeighted");

			// Allocate device buffers and transfer input data to device.

			Mat c = source.clone();
			cl::Buffer Src(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols*source.channels() * sizeof(uchar), source.data);

			cl::Buffer Dst(context, CL_MEM_WRITE_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols *source.channels() * sizeof(uchar), source.data);

			// Set kernel parameters.
			ContrastKernel.setArg(0, Src);
			ContrastKernel.setArg(1, Dst);
			ContrastKernel.setArg(2, static_cast<float>(alpha));
			ContrastKernel.setArg(3, static_cast<int>(beta));

			size_t N = source.rows*source.cols*source.channels();
			// Launch kernel on the compute device.
			queue.enqueueNDRangeKernel(ContrastKernel, cl::NullRange, N, cl::NullRange);

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

	void filter2D(Mat& source, Mat& kern, Point anchor, cl::Device device)
	{
		ifstream f("Filter2D.cl");
		string str((std::istreambuf_iterator<char>(f)),
				   std::istreambuf_iterator<char>());
		const char* contrastAndBrightnessSource = str.c_str();

		try
		{
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

			cl::Kernel FilterKernel(program, "Filter2D");

			// Allocate device buffers and transfer input data to device.

			Mat filtered = source.clone();
			cl::Buffer Src(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols*source.channels() * sizeof(uchar), source.data);

			cl::Buffer Dst(context, CL_MEM_WRITE_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols *source.channels() * sizeof(uchar), filtered.data);

			cl::Buffer Kern(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
						   kern.rows*kern.cols * sizeof(float), kern.data);

			// Set kernel parameters.
			FilterKernel.setArg(0, Src);
			FilterKernel.setArg(1, Kern);
			FilterKernel.setArg(2, Dst);
			FilterKernel.setArg(3, source.rows);
			FilterKernel.setArg(4, source.cols);
			FilterKernel.setArg(5, anchor.x);
			FilterKernel.setArg(6, anchor.y);
			FilterKernel.setArg(7, kern.rows);
			FilterKernel.setArg(8, kern.cols);
			FilterKernel.setArg(9, source.channels());

			size_t N = source.rows*source.cols*source.channels();
			// Launch kernel on the compute device.
			queue.enqueueNDRangeKernel(FilterKernel, cl::NullRange, N, cl::NullRange);

			// Get result back to host.
			queue.enqueueReadBuffer(Dst, CL_TRUE, 0, source.rows*source.cols *source.channels() * sizeof(uchar), filtered.data);

			source = filtered;
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