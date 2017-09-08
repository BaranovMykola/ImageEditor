#include "ImageProcess.h"

#include <iostream>

#include <opencv2\highgui.hpp>

using namespace cv;


//cv::Mat readOriginal(std::string fileName)
//{
//	Mat original = imread(fileName);
//	if (original.empty())
//	{
//		throw std::exception("Image load failed");
//	}
//	return original;
//}

cv::Mat RotateAt(const cv::Mat & img, float grad)
{
	//Computing affine matrix
	auto center = Point2f(img.cols / 2, img.rows / 2);
	cv::Mat rotateMat = getRotationMatrix2D(center, grad, 1);
	auto rotRect = RotatedRect(center, img.size(), grad).boundingRect();
	rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
	rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;

	cv::Mat rotatedImg;
	warpAffine(img, rotatedImg, rotateMat, rotRect.size());
	return rotatedImg;
}

cv::Mat resizeImg(const cv::Mat & img, float percentRatio)
{
	Size newSize(img.size().width*percentRatio, img.size().height*percentRatio);
	Mat resized;
	resize(img, resized, newSize);
	return resized;
}

cv::Mat contrastAndBrightness(const cv::Mat & img, float contrast, int brightness)
{
	Mat edited;
	img.convertTo(edited, edited.type(), contrast, brightness);
	return edited;
}