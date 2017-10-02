#pragma once
#include "AbstractChange.h"
#include <opencv2\imgproc.hpp>

class ResizeChange : public AbstractChange
{
public:
	ResizeChange(float _percentRatio)
	{
		percentRatio = _percentRatio;
	}
	float percentRatio;
	void apply(cv::Mat& source)
	{
		cv::Size newSize(source.size().width*percentRatio, source.size().height*percentRatio);
		cv::Mat resized = cv::Mat::zeros(newSize, CV_8UC3);
		cv::resize(source, source, newSize);
	}
private:

};