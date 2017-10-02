#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>

#include "ImageProcess.h"
#include "ImageEditor.h"

int main()
{
	std::string act;
	ImageEditor edit("fox.jpg");
	do
	{
		cin >> act;
		if (act == "show")
		{
			imshow("", edit.source);
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
	}
	while (true);
	return 0;
}