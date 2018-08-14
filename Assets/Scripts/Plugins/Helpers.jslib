mergeInto(LibraryManager.library, {

	GetQueryStringParam: function (str) {
		var url = window.location.href;
		var name = Pointer_stringify(str);
		name = name.replace(/[\[\]]/g, '\\$&');
		var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
			results = regex.exec(url);
		var param;
		if (!results || !results[2]) {
			param = "";
		}
		else {
			param = decodeURIComponent(results[2].replace(/\+/g, ' '));
		}
		var bufferSize = lengthBytesUTF8(param) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(param, buffer, bufferSize);
		return buffer;
	}

});