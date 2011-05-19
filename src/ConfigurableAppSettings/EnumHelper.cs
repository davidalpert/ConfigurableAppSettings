using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ConfigurableAppSettings
{
	public static class EnumHelper
	{
		public static T ParseAs<T>( this string value )
		{
			return EnumHelper.ParseAs<T>( value, false );
		}

		public static T ParseAs<T>( this string value, bool ignoreCase, T defaultValue )
		{
			T result = defaultValue;
			try
			{
				result = value.ParseAs<T>( ignoreCase );
			}
			catch
			{
			}
			return result;
		}

		public static T ParseAs<T>( this string value, bool ignoreCase )
		{
			Type t = typeof( T );

			T enumType = (T)ParseAs( value, t, ignoreCase );

			return enumType;
		}

		public static object ParseAs( this string value, Type targetType, bool ignoreCase )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			value = value.Trim();

			if ( value.Length == 0 )
			{
				throw new ArgumentException( "Must specify valid information for parsing in the string.", "value" );
			}

			if ( !targetType.IsEnum )
			{
				throw new ArgumentException( "Type provided must be an Enum.", "T" );
			}

			object enumType = Enum.Parse( targetType, value, ignoreCase );
			return enumType;
		}

		public static T ParseAsEnumByDescription<T>( this string value, T defaultValue )
		{
			return EnumHelper.ParseByDescription<T>( value, defaultValue, false );
		}

		public static T ParseByDescription<T>( this string value, T defaultValue, bool ignoreCase )
		{
			foreach ( T t in EnumHelper.GetValues<T>() )
			{
				string desc = t.GetDescription();
				if ( ( ignoreCase && desc.ToLowerInvariant().Equals( value.ToLowerInvariant() ) )
							|| desc.Equals( value ) )
				{
					return t;
				}
			}

			return defaultValue;
		}

		public static string GetDescription<T>( this T val )
		{
			var attr = val.GetAttribute<DescriptionAttribute>();
			if ( attr != null )
			{
				return attr.Description;
			}

			if ( val != null )
			{
				return val.ToString();
			}

			return String.Empty;
		}

		public static TAttribute GetAttribute<TAttribute>( this object val ) where TAttribute : Attribute
		{
			return val.GetType()
						.GetField( val.ToString() )
						.GetCustomAttributes( typeof( TAttribute ), false )
						.Cast<TAttribute>()
						.SingleOrDefault();
		}

		public static IEnumerable<T> GetValues<T>()
		{
			Type t = typeof( T );
			if ( !t.IsEnum )
			{
				throw new ArgumentException( "Type provided must be an Enum.", "T" );
			}
			return Enum.GetValues( t ).Cast<T>();
		}
	}
}
