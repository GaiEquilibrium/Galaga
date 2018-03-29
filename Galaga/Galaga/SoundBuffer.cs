using System;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace Galaga
{
    //звуковой буфер, содержит воспроизводимый буфер
    class SoundBuffer
    {
        private string _filename = "SoundTest.wav";
        private int _soundBuffer;

        public SoundBuffer()
        {
            _soundBuffer = BufferSetup();
        }
        public SoundBuffer(string soundFile)
        {
            _filename = soundFile;
            _soundBuffer = BufferSetup();
        }
        private byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riffChunckSize = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string formatSignature = new string(reader.ReadChars(4));
                if (formatSignature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int formatChunkSize = reader.ReadInt32();
                int audioFormat = reader.ReadInt16();
                int numChannels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                int byteRate = reader.ReadInt32();
                int blockAlign = reader.ReadInt16();
                int bitsPerSample = reader.ReadInt16();

                string dataSignature = new string(reader.ReadChars(4));
                if (dataSignature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int dataChunkSize = reader.ReadInt32();

                channels = numChannels;
                bits = bitsPerSample;
                rate = sampleRate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
        private ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
        private int BufferSetup()
        {
            int buffer;

            buffer = AL.GenBuffer();
            int channels, bitsPerSample, sampleRate;
            byte[] soundData = LoadWave(File.Open(_filename, FileMode.Open), out channels, out bitsPerSample, out sampleRate);
            AL.BufferData(buffer, GetSoundFormat(channels, bitsPerSample), soundData, soundData.Length, sampleRate);

            return buffer;
        }
        public int GetSoundBuffer() { return _soundBuffer; }
    }
}
