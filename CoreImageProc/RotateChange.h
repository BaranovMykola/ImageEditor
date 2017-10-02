#pragma once
#include "AbstractChange.h"
#include "opencv2\highgui.hpp"
#include "opencv2\imgproc.hpp"
#include "ImageProcessing.h"

using namespace cv;

class RotateChange : public AbstractChange
{
public:
	RotateChange(int _angle)
	{
		rotateAngle = _angle;
	}
	void apply(cv::Mat& source)
	{
		imp::rotate(source, rotateAngle);
	}
private:
	int rotateAngle;
};