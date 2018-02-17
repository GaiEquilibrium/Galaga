using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Collections.Generic;

namespace Galaga
{
    class SoundMaster
    {
        private Dictionary<int, int> sourceBuffer = new Dictionary<int, int>();//source = key / buffer = value

        public void SoundPlay() //запихивать это в отдельный поток?
        {
            using (AudioContext context = new AudioContext())
            {
                //setup
                SoundBuffer shootBuffer;
                bool isAll = false;

                //main cycle
                while (true)
                {
                    if (GlobalVariables.ShootFlag < 0) break;
                    //creating new sounds
                    if (GlobalVariables.ShootFlag > 0)
                    {
                        shootBuffer = new SoundBuffer();
                        int source = AL.GenSource();
                        AL.Source(source, ALSourcei.Buffer, shootBuffer.GetSoundBuffer());

                        sourceBuffer.Add(source, shootBuffer.GetSoundBuffer());

                        AL.SourcePlay(source);
                        GlobalVariables.ShootFlagDec();
                    }

                    isAll = false;
                    while (!isAll)
                    {
                        isAll = true;
                        foreach (KeyValuePair<int, int> sourceBufferPair in sourceBuffer)
                        {
                            int state;
                            // Query the source to find out when it stops playing.
                            AL.GetSource(sourceBufferPair.Key, ALGetSourcei.SourceState, out state);
                            if ((ALSourceState)state != ALSourceState.Playing)
                            {
                                AL.SourceStop(sourceBufferPair.Key);
                                AL.DeleteSource(sourceBufferPair.Key);
                                AL.DeleteBuffer(sourceBufferPair.Value);
                                sourceBuffer.Remove(sourceBufferPair.Key);
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
                    foreach (KeyValuePair<int, int> sourceBufferPair in sourceBuffer)
                    {
                        int state;
                        // Query the source to find out when it stops playing.
                        AL.GetSource(sourceBufferPair.Key, ALGetSourcei.SourceState, out state);
                        if ((ALSourceState)state != ALSourceState.Playing)
                        {
                            AL.SourceStop(sourceBufferPair.Key);
                            AL.DeleteSource(sourceBufferPair.Key);
                            AL.DeleteBuffer(sourceBufferPair.Value);
                            sourceBuffer.Remove(sourceBufferPair.Key);
                            isAll = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
