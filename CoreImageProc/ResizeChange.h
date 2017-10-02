#pragma once
#include "AbstractChange.h"
#include "ImageProcessing.h"
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
		imp::resize(source, percentRatio);
	}
private:

};