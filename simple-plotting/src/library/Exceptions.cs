namespace simple_plotting {
	public class ImageExtensionEnumIsAllException : Exception {
		public override string Message => simple_plotting.Message.EXCEPTION_CANNOT_RETURN_IMG_EXT_ALL;
	}
	
	public class NoBitmapParserException : Exception {
		public override string Message => simple_plotting.Message.EXCEPTION_NO_BITMAP_PARSER;
	}
	
	public class BitMapParserDisposedException : Exception {
		public override string Message => simple_plotting.Message.EXCEPTION_BITMAP_PARSER_DISPOSED;
	}

	public class ImageGrabberNotPrimedException : Exception {
		public override string Message => simple_plotting.Message.EXCEPTION_IMAGE_GRABBER_NOT_PRIMED;
	}

	public class NoImageParserAssignedException : Exception {
		public override string Message=>  simple_plotting.Message.EXCEPTION_NO_IMAGE_PARSER_ASSIGNED;
	}
}