#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>
#include <vector>

#include "AbstractChange.h"
#include "ContrastAndBrightnessChange.h"
#include "RotateChange.h"

using namespace cv;

class ImageEditor
{
public:
	ImageEditor(std::string file)
	{
		original = imread(file);
		source = original.clone();
	}
	void changeContrastAndBrightness(float _contrast, int _brightness)
	{
		source.convertTo(source, source.type(), _contrast, _brightness);
		changes.push_back(new ContrastAndBrightnessChange(_contrast, _brightness));
	}

	void restore(int step)
	{
		source = original.clone();
		for (int i = 0; i < step; i++)
		{
			changes[i]->apply(source);
		}
		changes.erase(changes.begin() + step + 1, changes.end());
	}

	void rotate(int angle)
	{
		auto center = Point2f(source.cols / 2, source.rows / 2);
		cv::Mat rotateMat = getRotationMatrix2D(center, angle, 1);
		auto rotRect = RotatedRect(center, source.size(), angle).boundingRect();
		rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
		rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;

		cv::Mat rotatedImg;
		warpAffine(source, rotatedImg, rotateMat, rotRect.size(), 1, BORDER_TRANSPARENT);
		source = rotatedImg;
		changes.push_back(new RotateChange(angle));
	}

public:
	Mat source;
	Mat original;
	std::vector<AbstractChange*> changes;
};
