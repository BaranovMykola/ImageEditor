#include "ImageProcess.h"

#include <opencv2\highgui.hpp>

using namespace cv;


cv::Mat readOriginal(std::string fileName)
{
	Mat original = imread(fileName);
	if (original.empty())
	{
		throw std::exception("Image load failed");
	}
	return original;
}
