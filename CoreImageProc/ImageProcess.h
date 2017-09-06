#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>

#include <string>

using namespace std;
using namespace cv;

static cv::Mat readOriginal(std::string fileName)
{
		Mat original = imread(fileName);
		if (original.empty())
		{
			throw std::exception("Image load failed");
		}
		return original;
}

