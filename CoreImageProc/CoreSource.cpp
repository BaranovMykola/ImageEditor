#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>

int main()
{
	cv::Mat img = cv::imread("img.jpg");
	cv::imshow("img", img);
	cv::waitKey();
	return 0;
}