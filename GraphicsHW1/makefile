PHONY: run build clean

run : build
	@./Graphics.exe ${ARGS}

build : App.config Clipper.cs GraphicsHW.csproj Line.cs Matrix2.cs PixelBuffer.cs Primitive.cs Vector2.cs Arguments.cs Driver.cs PostscriptReader.cs Properties/AssemblyInfo.cs Matrix3.cs Polygon.cs Transformations.cs Vector3.cs
	@dmcs *.cs -out:Graphics.exe
clean : 
	@rm Graphics.exe
