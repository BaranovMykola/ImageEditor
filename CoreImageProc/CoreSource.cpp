#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>

#include "ImageProcess.h"

int main()
{
	cv::Mat img = imread("img.jpg");
	int grad = 0;
	int b = 255;
	int c = 100;
	int size = 100;
	namedWindow("Panel");
	createTrackbar("RotateAngle", "Panel", &grad, 360);
	createTrackbar("B", "Panel", &b, 510);
	createTrackbar("C", "Panel", &c, 300);
	createTrackbar("Size", "Panel", &size, 300);
	CoreImgEditor cie("img.jpg");
	while (waitKey(30) != 27)
	{
		cie.editImage(size / 100.0, grad, c / 100.0, b - 255);
		cie.updatePreview(300, 300);
		cv::imshow("img", cie.getPreview());
	}
	cv::waitKey();
	return 0;
}