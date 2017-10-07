#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>
#include <vector>

#include "AbstractChange.h"
#include "ContrastAndBrightnessChange.h"
#include "RotateChange.h"
#include "ResizeChange.h"
#include "ImageProcessing.h"

using namespace cv;

class ImageEditor
{
public:
	ImageEditor()
	{

	}
	
	ImageEditor(std::string file)
	{
		loadImage(file);
	}

	void loadImage(std::string file)
	{
		original = imread(file);
		source = original.clone();
		preview = source.clone();
	}

	cv::Mat getSource()const
	{
		return source;
	}

	void changeContrastAndBrightness(float _contrast, int _brightness)
	{
		source.convertTo(preview, source.type(), _contrast, _brightness);
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
		preview = source.clone();
	}

	void resize(float percentRatio)
	{
		preview = source.clone();
		imp::resize(preview, percentRatio);
		changes.push_back(new ResizeChange(percentRatio));
	}

	void rotate(int angle)
	{
		preview = source.clone();
		imp::rotate(preview, angle);
		changes.push_back(new RotateChange(angle));
	}

	void apply()
	{
		source = preview.clone();
	}

	Mat getPreview()const
	{
		return preview;
	}

public:
	Mat preview;
	Mat source;
	Mat original;
	std::vector<AbstractChange*> changes;
};
