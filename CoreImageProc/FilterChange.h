#pragma once
#include "AbstractChange.h"
#include "ImageProcessing.h"

#include <opencv2\core.hpp>

class FilterChange : public AbstractChange
{
public:
	FilterChange(cv::Mat _kern, cv::Point _anchor):
		kern(_kern.clone()),
		anchor(_anchor)
	{

	}

	void apply(cv::Mat& source)
	{
		imp::filter2D(source, kern, anchor);
	}

private:
	cv::Mat kern;
	cv::Point anchor;
};