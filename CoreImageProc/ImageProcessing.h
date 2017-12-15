#pragma once
#include <opencv2\core.hpp>
#include <opencv2\imgproc.hpp>
#include <opencv2\highgui.hpp>

#include "opencv2/objdetect/objdetect.hpp"

using namespace cv;
namespace imp
{
	uchar** _getRowsPointers(Mat& mat);
	void clear_memory(uchar**, int);

	void resize(cv::Mat& source, float percentRatio)
	{
		cv::Size newSize(source.size().width*percentRatio, source.size().height*percentRatio);
		cv::Mat resized = Mat::zeros(newSize, source.type());
		cv::resize(source, source, newSize);
	}

	void rotate(cv::Mat& source, int angle)
	{
		auto center = Point2f(source.cols / 2, source.rows / 2);
		cv::Mat rotateMat = getRotationMatrix2D(center, angle, 1);
		auto rotRect = RotatedRect(center, source.size(), angle).boundingRect();
		rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
		rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;

		cv::Mat rotatedImg = Mat::zeros(source.size(), source.type());
		warpAffine(source, rotatedImg, rotateMat, rotRect.size() - Size(1, 1), 1, BORDER_TRANSPARENT);
		source = rotatedImg;
	}

	void palleting256(cv::Mat& source)
	{
		for (int i = 0; i < source.rows; i++)
		{
			for (int j = 0; j < source.cols*source.channels(); j++)
			{
				int val = source.at<uchar>(i, j);
				int diff = val % 43;
				val -= diff;
				source.at<uchar>(i, j) = val;
			}
		}
	}

	void filter2D(cv::Mat& source, cv::Mat& kern, cv::Point anchor)
	{
		Mat filtered;
		uchar** source_rows = _getRowsPointers(source);
		uchar* filtered_img_row;
		filtered = source.clone();
		for (int i = anchor.x; i < source.rows - kern.rows + anchor.x; ++i)
		{
			filtered_img_row = filtered.ptr<uchar>(i);
			for (int channel_num = 0; channel_num < source.channels(); ++channel_num)
			{
				for (int j = anchor.y + channel_num; j < (source.cols - kern.cols + anchor.y)*source.channels(); j += source.channels())
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
		clear_memory(source_rows, source.rows);
	}

	void toGrayscale(Mat& source)
	{
		if (source.type() == CV_8UC3)
		{
			uchar* curr_source_row;
			uchar* curr_res_row;
			Mat result(source.rows, source.cols, CV_8UC1);
			for (int i = 0; i < source.rows; ++i)
			{
				curr_source_row = source.ptr<uchar>(i);
				curr_res_row = result.ptr<uchar>(i);
				int result_j = 0;
				for (int j = 0; j < source.cols*source.channels(); j += source.channels())
				{
					curr_res_row[result_j] = (curr_source_row[j] + curr_source_row[j + 1] + curr_source_row[j + 2]) / source.channels();
					++result_j;
				}
			}
			source = result.clone();
		}
	}

	void printFaces(cv::Mat& source, std::vector<cv::Rect> faces)
	{
		for (auto i : faces)
		{
			cv::rectangle(source, i, cv::Scalar(0, 255, 0), 4);
		}
	}

	void changeContrastAndBrightness(Mat& source, Mat& res, float _contrast, int _brightness)
	{
		res = source.clone();
		uchar* curr_row_source;
		uchar* curr_row_res;
		for (size_t i = 0; i < source.rows; ++i)
		{
			curr_row_source = source.ptr<uchar>(i);
			curr_row_res = res.ptr<uchar>(i);
			for (size_t j = 0; j < source.cols*source.channels(); ++j)
			{
				curr_row_res[j] = saturate_cast<uchar> (_contrast*curr_row_source[j] + _brightness);
			}
		}
	}

	std::vector<cv::Rect> detectFace(Mat& source)
	{
		std::vector<Rect> faces;
		Mat frame_gray;

		Mat frame = source;
		if (frame.type() == CV_8UC1)
		{
			frame_gray = frame.clone();
		}
		else
		{
			cvtColor(frame, frame_gray, CV_BGR2GRAY);
		}

		equalizeHist(frame_gray, frame_gray);

		CascadeClassifier face_cascade;
		bool loaded = face_cascade.load("haarcascade_frontalface_alt2.xml");
		cout << "Loaded " << loaded << endl;


		//-- Detect faces
		face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

		printFaces(source, faces);

		return faces;
	}

	uchar** _getRowsPointers(Mat& mat)
	{
		uchar** result;
		result = new uchar*[mat.rows];
		for (int i = 0; i < mat.rows; ++i)
		{
			result[i] = mat.ptr<uchar>(i);
		}
		return result;
	}

	void clear_memory(uchar** mat_rows, int rows_quantity)
	{
		delete[] mat_rows;
	}
}

#include <iostream>
#include <vector>
#include <string>

#define __CL_ENABLE_EXCEPTIONS
#include <CL/cl.hpp>

void filter2D_cuda(Mat& source, Mat& kern, Point anchor)
{
	static const char sourceProg[] =
		"#if defined(cl_khr_fp64)\n"
		"#  pragma OPENCL EXTENSION cl_khr_fp64: enable\n"
		"#elif defined(cl_amd_fp64)\n"
		"#  pragma OPENCL EXTENSION cl_amd_fp64: enable\n"
		"#else\n"
		"#  error double precision is not supported\n"
		"#endif\n"
		"kernel void Floyd(\n"
		"       global const unsigned char *Src,\n"
		"       global unsigned char *Dst,\n"
		"       float alpha,\n"
		"       int beta\n"
		"       )\n"
		"{\n"
		"    size_t i = get_global_id(0);\n"
		"    float res = (float)Src[i]+beta;\n"
		"	 float sature = 1;"
		"		if(res > 255) sature = 255;"
		"		else if(res < 0) sature = 30;"
		"		else sature = res;"
		"Dst[i] = sature;"
		"}\n";

	try
	{
		std::vector<cl::Platform> platform;
		cl::Platform::get(&platform);

		if (platform.empty())
		{
			std::cerr << "OpenCL platforms not found." << std::endl;
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
			std::cerr << "GPUs with double precision not found." << std::endl;
			throw;
		}

		std::cout << device[0].getInfo<CL_DEVICE_NAME>() << "\t";

		cl::CommandQueue queue(context, device[0]);

		cl::Program program(context, cl::Program::Sources(
			1, std::make_pair(sourceProg, strlen(sourceProg))
		));

		try
		{
			program.build(device);
		}
		catch (const cl::Error&)
		{
			std::cerr
				<< "OpenCL compilation error" << std::endl
				<< program.getBuildInfo<CL_PROGRAM_BUILD_LOG>(device[0])
				<< std::endl;
			throw;
		}

		cl::Kernel Floyd(program, "Floyd");

			// Allocate device buffers and transfer input data to device.

			Mat c = source.clone();
			cl::Buffer Src(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
						 source.rows*source.cols*sizeof(uchar), source.data);

			cl::Buffer Dst(context, CL_MEM_WRITE_ONLY | CL_MEM_COPY_HOST_PTR,
						   source.rows*source.cols * sizeof(uchar), c.data);

			//cl::Buffer B(context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
			//			 b.size() * sizeof(double), b.data());

			//cl::Buffer C(context, CL_MEM_READ_WRITE,
			//			 Wk_1.size() * sizeof(double));

			// Set kernel parameters.
			Floyd.setArg(0, Src);
			Floyd.setArg(1, Dst);
			Floyd.setArg(2, static_cast<float>(2));
			Floyd.setArg(3, static_cast<int>(-60));

			size_t N = source.rows*source.cols;
			// Launch kernel on the compute device.
			queue.enqueueNDRangeKernel(Floyd, cl::NullRange, N, cl::NullRange);

			// Get result back to host.
			queue.enqueueReadBuffer(Dst, CL_TRUE, 0, source.rows*source.cols * sizeof(uchar), c.data);


			//W_ = Wk_1;
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