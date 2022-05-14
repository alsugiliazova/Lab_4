// pch.cpp: файл исходного кода, соответствующий предварительно скомпилированному заголовочному файлу

#include "pch.h"

#define PI 3.14159265

// Serves to count time (is given in seconds)
class Timer
{
private:
	using clock_t = std::chrono::high_resolution_clock;
	using second_t = std::chrono::duration<double, std::ratio<1> >;

	std::chrono::time_point<clock_t> m_beg;

public:
	Timer() : m_beg(clock_t::now())
	{
	}

	void reset()
	{
		m_beg = clock_t::now();
	}

	double elapsed() const
	{
		return std::chrono::duration_cast<second_t>(clock_t::now() - m_beg).count();
	}
};

extern "C" _declspec(dllexport) double GlobMKLFunc(const int length, const double* vector, const int CurFunction, double* res1, double* res2, double* res3, double* res4)
{
	if (CurFunction == 0)
	{
		// IDK
		int n = length;
		long long mode = VML_HA;
		// Timer Class
		Timer t;

		// vmdtan VML_HA
		vmdtan(&n, vector, res1, &mode);
		res4[0] = t.elapsed();

		// vmdtan VML_EP
		mode = VML_EP;
		t.reset();
		vmdtan(&n, vector, res2, &mode);
		res4[1] = t.elapsed();

		// tan
		t.reset();
		for (int i = 0; i < length; i++)
		{
			res3[i] = tan(vector[i] * PI / 180.0);
		}
		res4[2] = t.elapsed();

		return 0;
	}
	else if (CurFunction == 1)
	{
		// IDK
		int n = length;
		long long mode = VML_HA;
		// Timer Class
		Timer t;

		// ErfInv VML_HA
		vmdErfInv(n, vector, res1, mode);
		res4[0] = t.elapsed();

		// ErfInv VML_EP
		mode = VML_EP;
		//mode = VML_LA;
		t.reset();
		vmdErfInv(n, vector, res2, mode);
		res4[1] = t.elapsed();

		// Erf
		t.reset();
		for (int i = 0; i < length; i++)
		{
			res3[i] = erf(vector[i]);
		}
		res4[2] = t.elapsed();

		return 0;
	}
	else
	{
		return -1;
	}
}


