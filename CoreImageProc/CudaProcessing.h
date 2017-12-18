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

	void filter2D(Mat& source, Mat& kern, Point anchor, cl::Device device)
	{

		/*Mat filtered;
		uchar** source_rows = _getRowsPointers(source);
		uchar* filtered_img_row;
		filtered = source.clone();
		for (int i = anchor.x; i < source.rows - kern.rows + anchor.x; ++i) // x->y
		{
			filtered_img_row = filtered.ptr<uchar>(i);
			for (int channel_num = 0; channel_num < source.channels(); ++channel_num)
			{
				for (int j = anchor.y + channel_num; j < (source.cols - kern.cols + anchor.y)*source.channels(); j += source.channels()) // y->x
				{
					double result = 0;
					for (int k = 0; k < kern.rows; ++k)
					{
						for (int l = 0; l < kern.cols; ++l)
						{
							result += (kern.at<float>(k, l)* source_rows[i + k - anchor.x][j + (l - anchor.y)*source.channels()]);
						}
					}
					filtered_img_row[j] = saturate_cast<uchar>(result);
				}
			}
		}
		source = filtered;
		clear_memory(source_rows, source.rows);*/

		ifstream f("Filter2D.cl");
		string str((std::istreambuf_iterator<char>(f)),
				   std::istreambuf_iterator<char>());
		const char* contrastAndBrightnessSource = str.c_str();

		try
		{
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
						   source.rows*source.cols *source.channels() * sizeof(uchar), c.data);

			cl::Buffer Kern(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
						   kern.rows*kern.cols * sizeof(float), kern.data);

			// Set kernel parameters.
			Floyd.setArg(0, Src);
			Floyd.setArg(1, Kern);
			Floyd.setArg(2, Dst);
			Floyd.setArg(3, source.rows);
			Floyd.setArg(4, source.cols);
			Floyd.setArg(5, anchor.x);
			Floyd.setArg(6, anchor.y);
			Floyd.setArg(7, kern.rows);
			Floyd.setArg(8, kern.cols);
			Floyd.setArg(9, source.channels());

			size_t N = source.rows*source.cols*source.channels();
			// Launch kernel on the compute device.
			queue.enqueueNDRangeKernel(Floyd, cl::NullRange, N, cl::NullRange);

			// Get result back to host.
			queue.enqueueReadBuffer(Dst, CL_TRUE, 0, source.rows*source.cols *source.channels() * sizeof(uchar), c.data);

			//imp::filter2D(source, kern, anchor);
			Mat f;
			cv::filter2D(source, f, source.depth(), kern, anchor);

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