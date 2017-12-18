#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>
#include <vector>
#include <iostream>

#include "AbstractChange.h"
#include "ContrastAndBrightnessChange.h"
#include "RotateChange.h"
#include "ResizeChange.h"
#include "PalletingChange.h"
#include "FilterChange.h"
#include "GrayscaleChange.h"
#include "FaceDetactionChange.h"
#include "ImageProcessing.h"
#include "CudaProcessing.h"

#include "opencv2/objdetect/objdetect.hpp"

using namespace cv;

class ImageEditor
{
public:
	ImageEditor()
	{
		currentChange = nullptr;
		searchGPUDevice();
	}
	
	ImageEditor(std::string file)
	{
		currentChange = nullptr;
		loadImage(file);
		searchGPUDevice();
	}

	void loadImage(std::string file)
	{
		original = imread(file);
		source = original.clone();
		preview = source.clone();
		changes.clear();
	}

	cv::Mat getSource()const
	{
		return source;
	}

	void changeContrastAndBrightness(float _contrast, int _brightness)
	{
		preview = source.clone();
		if (preview.cols*preview.rows*preview.channels() < 1000000)
		{
			cout << "CPU used" << endl;
			imp::changeContrastAndBrightness(preview, _contrast, _brightness);
		}
		else
		{
			try
			{
				cout << "GPU used" << endl;
				gpu::contratAndBirghtness_cuda(preview, _contrast, _brightness, GPUDevice);
			}
			catch (...)
			{
				cout << "GPU error. CPU used" << endl;
				imp::changeContrastAndBrightness(preview, _contrast, _brightness);
			}
		}

		eraseChange();
		currentChange = new ContrastAndBrightnessChange(_contrast, _brightness);
	}

	void restore(int step)
	{
		source = original.clone();
		for (int i = 0; i < step; i++)
		{
			changes[i]->apply(source);
		}
		changes.erase(changes.begin() + step + 1, changes.end());
		preview = source.clone();
	}

	void resize(float percentRatio)
	{
		preview = source.clone();
		imp::resize(preview, percentRatio);
		eraseChange();
		currentChange = new ResizeChange(percentRatio);
	}

	void rotate(int angle)
	{
		preview = source.clone();
		imp::rotate(preview, angle);
		eraseChange();
		currentChange = new RotateChange(angle);
	}

	void apply()
	{
		source = preview.clone();
		changes.push_back(currentChange);
		currentChange = nullptr;
	}

	Mat getPreview()const
	{
		return preview;
	}

	int getMinimum()const
	{
		double min;
		cv::minMaxLoc(source, &min, 0);
		return min;
	}

	std::vector<Mat> getPreviewIcons(float resizeRatio)
	{
		Mat originalCopy = original.clone();
		imp::resize(originalCopy, resizeRatio);

		std::vector<Mat> icons;
		icons.push_back(originalCopy.clone());
		for (int i = 0; i < changes.size(); i++)
		{
			changes[i]->apply(originalCopy);
			icons.push_back(originalCopy.clone());
		}

		for (auto i : icons)
		{
			imshow("change", i);
			waitKey();
		}
		return icons;
	}

	void save(std::string fileName)
	{
		if (original.empty())
		{
			imshow(fileName, Mat::zeros(300, 300, CV_8UC3));
		}
		imwrite(fileName, source);
	}

	void detectFace()
	{
		preview = source.clone();
		auto faces = imp::detectFace(preview);
		currentChange = new FaceDetectionChange(faces);
	}

	void palleting256()
	{
		preview = source.clone();
		imp::palleting256(preview);
		eraseChange();
		currentChange = new PalletingChange();
	}

	void filter(cv::Mat& kern, cv::Point anchor)
	{
		preview = source.clone();
		try
		{
			if (preview.cols*preview.rows > 250000 || kern.rows*kern.cols > 50)
			{
				cout << "GPU used" << endl;
				gpu::filter2D(preview, kern, anchor,GPUDevice);
			}
			else
			{
				cout << "CPU used" << endl;
				imp::filter2D(preview, kern, anchor);
			}
		}
		catch (...)
		{
			cout << "GPU error. CPU used" << endl;
			imp::filter2D(preview, kern, anchor);
		}
		eraseChange();
		currentChange = new FilterChange(kern, anchor);
	}

	void toGrayscale()
	{
		preview = source.clone();
		imp::toGrayscale(preview);
		eraseChange();
		currentChange = new GrayscalseChange();
	}

	void searchGPUDevice()
	{
		std::vector<cl::Platform> platform;
		cl::Platform::get(&platform);

		if (platform.empty())
		{
			throw;
		}

		cl::Context context;
		std::vector<cl::Device> devices;
		for (auto p = platform.begin(); devices.empty() && p != platform.end(); p++)
		{
			std::vector<cl::Device> pldev;

			try
			{
				p->getDevices(CL_DEVICE_TYPE_GPU, &pldev);

				for (auto d = pldev.begin(); devices.empty() && d != pldev.end(); d++)
				{
					if (!d->getInfo<CL_DEVICE_AVAILABLE>()) continue;

					std::string ext = d->getInfo<CL_DEVICE_EXTENSIONS>();

					if (
						ext.find("cl_khr_fp64") == std::string::npos &&
						ext.find("cl_amd_fp64") == std::string::npos
						) continue;

					devices.push_back(*d);
					context = cl::Context(devices);
				}
			}
			catch (...)
			{
				devices.clear();
			}
			if (!devices.empty())
			{
				GPUDevice = devices[0];
			}
		}
	}

public:

	void eraseChange()
	{
		if (currentChange != nullptr)
		{
			delete currentChange;
		}
	}
	Mat preview;
	Mat source;
	Mat original;
	std::vector<AbstractChange*> changes;
	AbstractChange* currentChange;
	cl::Device GPUDevice;
};
