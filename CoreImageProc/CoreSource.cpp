#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>
#include "opencv2/objdetect/objdetect.hpp"
#include "ImageProcess.h"
#include "ImageEditor.h"

int main()
{
	std::string act;
	ImageEditor edit("img.jpg");
	do
	{
		cin >> act;
		if (act == "show")
		{
			imshow("", edit.preview);
			waitKey();
			destroyAllWindows();
		}
		else if (act == "change")
		{
			float c;
			int b;
			cin >> c >> b;
			edit.changeContrastAndBrightness(c, b);
		}
		else if (act == "rotate")
		{
			int a;
			cin >> a;
			edit.rotate(a);

		}
		else if (act == "resize")
		{
			float p;
			cin >> p;
			edit.resize(p);
		}
		else if (act == "restore")
		{
			int s;
			cin >> s;
			edit.restore(s);
		}
		else if (act == "apply")
		{
			edit.apply();
		}
		else if (act == "history")
		{
			edit.getPreviewIcons(0.5);
		}
		else if (act == "detect")
		{
			edit.detectFace();
		}
		else if (act == "bmp")
		{
			edit.paletting256();
		}
	}
	while (true);
	return 0;
}