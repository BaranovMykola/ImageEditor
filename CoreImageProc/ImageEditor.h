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
#include "ImageProcessing.h"
#include "FaceDetactionChange.h"

#include "opencv2/objdetect/objdetect.hpp"

using namespace cv;

class ImageEditor
{
public:
	ImageEditor()
	{
		currentChange = nullptr;
	}
	
	ImageEditor(std::string file)
	{
		currentChange = nullptr;
		loadImage(file);
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
		imp::changeContrastAndBrightness(source, preview, _contrast, _brightness);
		//preview = source.clone();

		//contratAndBirghtness_cuda(preview, _contrast, _brightness);
		//source.convertTo(preview, source.type(), _contrast, _brightness); // this is library func. you should replace it by your func
		/*
		             ^  :) ok?
		//instead of |
		preview = source.clone();
		imp::yourfunc(preview, _contrast,_brightness);
		*/
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
		imp::filter2D(preview, kern, anchor);
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
};
