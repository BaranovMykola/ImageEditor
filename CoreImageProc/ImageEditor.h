#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>
#include <vector>

#include "AbstractChange.h"
#include "ContrastAndBrightnessChange.h"

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

public:
	Mat source;
	Mat original;
	std::vector<AbstractChange*> changes;
};
