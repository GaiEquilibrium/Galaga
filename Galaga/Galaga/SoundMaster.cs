using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Collections.Generic;

namespace Galaga
{
    //отвечает непосредственно за воспроизведение звуков
    //по сути работает с источниками звука, используя буферы
    class SoundMaster
    {
        //TODO
        //разобраться с щёлканьем в конце
        //вероятно необходимо переписать во имя упрощения кода и улучшения работы
        private Dictionary<int, int> _sourceBuffer = new Dictionary<int, int>();//source = key / buffer = value

        private int ShootFlag = 0;

        public void SoundPlay()
        {
            using (AudioContext context = new AudioContext())
            {
                //setup
                SoundBuffer shootBuffer;
                bool isAll = false;

                //main cycle
                while (true)
                {
                    if (ShootFlag < 0) break;
                    //creating new sounds
                    if (ShootFlag > 0)
                    {
                        shootBuffer = new SoundBuffer();
                        int source = AL.GenSource();
                        AL.Source(source, ALSourcei.Buffer, shootBuffer.GetSoundBuffer());

                        _sourceBuffer.Add(source, shootBuffer.GetSoundBuffer());

                        AL.SourcePlay(source);
                        ShootFlag--;
                    }

                    isAll = false;
                    while (!isAll)
                    {
                        isAll = true;
                        foreach (KeyValuePair<int, int> sourceBufferPair in _sourceBuffer)
                        {
                            int state;
                            // Query the source to find out when it stops playing.
                            AL.GetSource(sourceBufferPair.Key, ALGetSourcei.SourceState, out state);
                            if ((ALSourceState)state != ALSourceState.Playing)
                            {
                                AL.SourceStop(sourceBufferPair.Key);
                                AL.DeleteSource(sourceBufferPair.Key);
                                AL.DeleteBuffer(sourceBufferPair.Value);
                                _sourceBuffer.Remove(sourceBufferPair.Key);
                                isAll = false;
                                break;
                            }
                        }
                    }
                }
                isAll = false;
                while (!isAll)
                {
                    isAll = true;
                    foreach (KeyValuePair<int, int> sourceBufferPair in _sourceBuffer)
                    {
                        int state;
                        // Query the source to find out when it stops playing.
                        AL.GetSource(sourceBufferPair.Key, ALGetSourcei.SourceState, out state);
                        if ((ALSourceState)state != ALSourceState.Playing)
                        {
                            AL.SourceStop(sourceBufferPair.Key);
                            AL.DeleteSource(sourceBufferPair.Key);
                            AL.DeleteBuffer(sourceBufferPair.Value);
                            _sourceBuffer.Remove(sourceBufferPair.Key);
                            isAll = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
