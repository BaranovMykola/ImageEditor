#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>

#include "ImageProcess.h"

int main()
{
	cv::Mat img = readOriginal("img.jpg");
	cv::imshow("img", img);
	cv::waitKey();
	return 0;
}