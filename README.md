# Unity Log Recorder
Utility for recording Unity GameObjects' properties for later evaluation and playback.
Allows to log properties like position or time together with annotations as `csv` files and provides a simple GUI to playback the recorded states like a video inside the Unity Editor.

## When to use
This utility was developed for the recording of user studies in HCI experiments powered by Unity applications.\
In general it is useful in all kinds of settings where a resimulation or playback of virtual objects over time is needed.\
For example in an experiment, it is useful to log how virtual objects were moved and annotate these logs with experimental conditions or timestamps. This allows researchers to later evaluate these logs and helps to determine the cause of outliers in certain conditions.

## How to use
### LoggerController
To log the properties of a GameObject in Unity, it needs a `LoggerController` componenent attached which bundles the all `Loggers` results into a single `csv` file. It provides the following settings:
* `Save Log`: If this component should record and save its logs for the next time running the application.
* `File Name`: Name of saved file, will be saved to `Assets/CSV/<name>.csv`.
* `Log Mode`: Log entries can be created `EACH_FRAME`, every `Wait Interval` frames (`WAIT_INTERVAL`) or only when the trigger `LoggerController.Trigger()` is called by other scripts.

### Loggers
To record different properties, different components deriving from type `Logger` are used. Currently the following types are already implemented (though this list can easily grow):
* `FrameLogger` is a special type and will be created automatically if none is found on the `LoggerController`'s GameObject.
* `AnnotationLogger`is a special type used to annotate the log with a message.
* `PositionLogger`
* `RotationLogger`
* `ScaleLogger`
* `TimeLogger`

### Create a custom logger
TODO

### Add annotations
TODO

### How to playback
TODO
