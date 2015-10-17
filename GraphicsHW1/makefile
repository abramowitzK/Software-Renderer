PHONY: run build clean

run : build
	@./Graphics.exe ${ARGS}

build : App.config Clipper.cs GraphicsHW.csproj Line.cs Matrix2.cs PixelBuffer.cs Primitive.cs Vector2.cs Arguments.cs Driver.cs PostscriptReader.cs Properties/AssemblyInfo.cs 
	@dmcs *.cs -out:Graphics.exe
clean : 
	@rm Graphics.exe
