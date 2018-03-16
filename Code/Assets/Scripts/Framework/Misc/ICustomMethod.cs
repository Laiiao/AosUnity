﻿namespace framework
{
	public interface IInstanceMethod
	{
		void Run();
		void Run(object a);
		void Run(object a, object b);
		void Run(object a, object b, object c);
	}

	public interface IStaticMethod
	{
		void Run();
		void Run(object a);
		void Run(object a, object b);
		void Run(object a, object b, object c);
	}
}
