//using System;
//using System.Reflection;

//namespace Earl.Crawler.Infrastructure.Abstractions
//{

//    [AttributeUsage( AttributeTargets.Class )]
//    public class DependsOnRequestMiddlewareAttribute : Attribute
//    {
//        #region Fields
//        private static readonly Type MiddlewareInterface = typeof( ICrawlRequestMiddleware ).GetTypeInfo();
//        private readonly Type middlewareType;
//        #endregion

//        #region Properties
//        public Type MiddlewareType => middlewareType;
//        #endregion

//        public DependsOnRequestMiddlewareAttribute( Type middlewareType )
//        {
//            if( !middlewareType.IsAssignableTo( MiddlewareInterface ) )
//            {
//                throw new ArgumentException( $"Given type is not of {nameof( ICrawlRequestMiddleware )}.", nameof( middlewareType ) );
//            }

//            this.middlewareType = middlewareType;
//        }

//    }

//}
