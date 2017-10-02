#pragma once
#include <opencv2\core.hpp>

class AbstractChange
{
public:
	virtual void apply(cv::Mat& source)=0;
private:

};