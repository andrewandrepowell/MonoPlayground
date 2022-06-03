# -*- coding: utf-8 -*-
"""
Spyder Editor

This is a temporary script file.
"""
import aifc
import numpy
import sounddevice
from matplotlib import pyplot
from scipy.io import wavfile
import pydub


def write(file: str, data: numpy.ndarray, frame_rate: int) -> None:
    assert len(data.shape) == 1 or len(data.shape) == 2
    assert data.dtype == numpy.int32
    song = pydub.AudioSegment(data.tobytes(), frame_rate=frame_rate, sample_width=4, channels=data.shape[1])
    song.export(file, format="mp3", bitrate="320k")


if __name__ == "__main__":
    FULL_SOUND_NAME = "110930__bennychico11__8bitsfx.aiff"
    
    # Load the sound AIFF file.
    with aifc.open(f=FULL_SOUND_NAME) as full_sound:
        
        # Get parameters from the sound packs. Print out the information.
        channels_per_sound, bytes_per_sample, frame_rate, frames_per_sound, _, _ = full_sound.getparams()
        print(f"channels_per_sound: {channels_per_sound}")
        print(f"bytes_per_sample: {bytes_per_sample}")
        print(f"frame_rate: {frame_rate}")
        print(f"frames_per_sound: {frames_per_sound}")
        
        # Get the raw data, and print out more information.
        raw_data = full_sound.readframes(frames_per_sound)
        print(f"len(raw_data)={len(raw_data)}")
        
        # Convert the data into numpy array.
        data = []
        for i, b in enumerate(raw_data):
            offset = i % bytes_per_sample
            channel = (i // bytes_per_sample) % channels_per_sound
            if offset == 0:
                sample = 0
            sample = (sample << 8) | b
            if offset == (bytes_per_sample - 1):
                data.append(sample)
        data = numpy.array(data).astype(numpy.int32).reshape((frames_per_sound, channels_per_sound))
        
        # Play the entire sound pack.
        # sounddevice.play(data, frame_rate)
        
        # Graph the sound.
        #pyplot.plot(data)
        
        # Grab the run sound.
        # run_data = data[245000:300000,:]
        # run_data = data[248000:254000,:]
        # run_data = data[250100:252000,:]
        # sounddevice.play(run_data, frame_rate)
        # wavfile.write('run.wav', frame_rate, run_data)
        # write(file='run.mp3', data=run_data, frame_rate=frame_rate)
        # pyplot.plot(run_data)
        
        white_noise = numpy.random.normal(loc=0, scale=17500000, size=(60000, channels_per_sound)).astype(numpy.int32)
        sounddevice.play(white_noise, frame_rate)
        pyplot.plot(white_noise)
        write(file='noiseSound.mp3', data=white_noise, frame_rate=frame_rate)
        
        pyplot.show()
        pass
