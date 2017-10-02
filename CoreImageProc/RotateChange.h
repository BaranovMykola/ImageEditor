#pragma once
#include "AbstractChange.h"
#include "opencv2\highgui.hpp"
#include "opencv2\imgproc.hpp"

using namespace cv;

class RotateChange : public AbstractChange
{
public:
	RotateChange(int _angle)
	{
		rotateAngle = _angle;
	}
	int rotateAngle;
	void apply(cv::Mat& source)
	{
		auto center = Point2f(source.cols / 2, source.rows / 2);
		cv::Mat rotateMat = getRotationMatrix2D(center, rotateAngle, 1);
		auto rotRect = RotatedRect(center, source.size(), rotateAngle).boundingRect();
		rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
		rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;

		cv::Mat rotatedImg;
		warpAffine(source, rotatedImg, rotateMat, rotRect.size(), 1, BORDER_TRANSPARENT);
		source = rotatedImg;
	}
private:

};