#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>

using namespace std;
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

cv::Mat RotateAt(const cv::Mat& img, float grad);

cv::Mat resizeImg(const cv::Mat& img, float percent);

cv::Mat contrastAndBrightness(const cv::Mat& img, float contrast, int brightness);

enum Side { WIDTH, HEIGHT };

cv::Mat convertToPreview(cv::Mat & sourceImg, int sideLenght, int side)
{
	cv::Size originalSize = sourceImg.size();
	float ratio;
	switch (side)
	{
		case Side::WIDTH:
			ratio = originalSize.width / (float)sideLenght;
			break;
		case Side::HEIGHT:
			ratio = originalSize.height/ (float)sideLenght;
			break;
		default:
			ratio = 1;
			break;
	}

	cv::Size newSize (originalSize.width*ratio, originalSize.height*ratio);
	cv::Mat preview;
	int naturalRatio = ratio;
	if (naturalRatio % 2 != 0 && naturalRatio> 1)
	{
		--naturalRatio;
	}

	for (int i = 0; i < naturalRatio; ++++i)
	{
		pyrDown(sourceImg, preview);
		sourceImg = preview;
	}
	return preview;
}

