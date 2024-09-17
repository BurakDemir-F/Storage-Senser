<h1>Storage Senser</h1>
CLI tool for scanning directories to find large sized files based on parameters. Program starts working on current directory and continue to scan with the folder tree. 
Has two parameters <b>threshold</b> and <b>parallel</b>.

<b>threshold:</b> identifies the file limit program, for example if you give 10 it will find files bigger than 10 mb.

<b>parallel:</b> optional parameter for using parallel execution, for scanning big file storage it is faster.

<h2> Examples:</h2>

<b>StorageSenser --threshold 100</b> : finds files bigger than 100 mb starting with current working directory".

<b>StorageSenser --threshold 100 --parallel</b> : finds files bigger than 100 mb starting with current working directory, uses parallel execution

<a href = "https://github.com/mayuki/Cocona">Cocona</a> used for handling command line arguments.
