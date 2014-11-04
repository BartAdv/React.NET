﻿/*
 *  Copyright (c) 2014, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

using System.Web;
using System.Web.Mvc;

namespace React.Web.Mvc
{
	using AssemblyRegistration = React.AssemblyRegistration;

	/// <summary>
	/// HTML Helpers for utilising React from an ASP.NET MVC application.
	/// </summary>
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// Gets the React environment
		/// </summary>
		private static IReactEnvironment Environment
		{
			// TODO: Figure out if this can be injected
			get { return AssemblyRegistration.Container.Resolve<IReactEnvironment>(); }
		}

		/// <summary>
		/// Renders the specified React component
		/// </summary>
		/// <typeparam name="T">Type of the props</typeparam>
		/// <param name="htmlHelper">HTML helper</param>
		/// <param name="componentName">Name of the component</param>
		/// <param name="props">Props to initialise the component with</param>
		/// <param name="initialize">Whether to include a script tag to inilalize component. Optional, defaults to false.</param>
		/// <returns>The component's HTML</returns>
		public static IHtmlString React<T>(
			this HtmlHelper htmlHelper, 
			string componentName, 
			T props,
			bool initialize=false
		)
		{
			var reactComponent = Environment.CreateComponent(componentName, props);
			var result = reactComponent.RenderHtml();
			if (initialize)
			{
				var script = new TagBuilder("script")
				{
					InnerHtml = reactComponent.RenderJavaScript()
				};
				return new HtmlString(result + "\n" + script.ToString());
			}
			else
			{
				return new HtmlString(result);
			}
		}

		/// <summary>
		/// Renders the JavaScript required to initialise all components client-side. This will 
		/// attach event handlers to the server-rendered HTML.
		/// </summary>
		/// <returns>JavaScript for all components</returns>
		public static IHtmlString ReactInitJavaScript(this HtmlHelper htmlHelper)
		{
			var script = Environment.GetInitJavaScript();
			var tag = new TagBuilder("script")
			{
				InnerHtml = script
			};
			return new HtmlString(tag.ToString());
		}
	}
}
