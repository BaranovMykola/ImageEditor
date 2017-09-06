#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>

using namespace std;
using namespace cv;

static cv::Mat readOriginal(std::string fileName)
{
		Mat original = imread(fileName);
		return original;
}

