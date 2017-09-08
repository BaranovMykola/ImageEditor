#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>

#include "ImageProcess.h"

int main()
{
	cv::Mat img = readOriginal("img.jpg");
	int grad = 100;
	int b = 0;
	namedWindow("Panel");
	createTrackbar("RotateAngle", "Panel", &grad, 360);
	createTrackbar("b", "Panel", &b, 510);
	while (waitKey(30) != 27)
	{
		auto resized = contrastAndBrightness(img, grad/100.0, b-255);
		cv::imshow("img", resized);
	}
	cv::waitKey();
	return 0;
}