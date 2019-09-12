using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static readonly string swapPuzzleSavePath = Application.persistentDataPath + "/SwapPuzzle.save";
    public static readonly string rotatePuzzleSavePath = Application.persistentDataPath + "/RotatePuzzle.save";
    public static readonly string slidePuzzleSavePath = Application.persistentDataPath + "/SlidePuzzle.save";

    public static void SaveRotatePuzzle(RotatePuzzle rotatePuzzle)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(rotatePuzzleSavePath, FileMode.Create);

        RotatePuzzleData data = new RotatePuzzleData(rotatePuzzle);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static RotatePuzzleData LoadRotatePuzzle()
    {
        if(File.Exists(rotatePuzzleSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(rotatePuzzleSavePath, FileMode.Open);

            RotatePuzzleData data = formatter.Deserialize(stream) as RotatePuzzleData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + rotatePuzzleSavePath);
            return null;
        }
    }

    public static void SaveSwapPuzzle(SwapPuzzle swapPuzzle)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(swapPuzzleSavePath, FileMode.Create);

        SwapPuzzleData data = new SwapPuzzleData(swapPuzzle);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveSlidePuzzle(SlidePuzzle slidePuzzle)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(slidePuzzleSavePath, FileMode.Create);

        SlidePuzzleData data = new SlidePuzzleData(slidePuzzle);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SwapPuzzleData LoadSwapPuzzle()
    {
        if (File.Exists(swapPuzzleSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(swapPuzzleSavePath, FileMode.Open);

            SwapPuzzleData data = formatter.Deserialize(stream) as SwapPuzzleData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + swapPuzzleSavePath);
            return null;
        }
    }

    public static SlidePuzzleData LoadSlidePuzzle()
    {
        if (File.Exists(slidePuzzleSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(slidePuzzleSavePath, FileMode.Open);

            SlidePuzzleData data = formatter.Deserialize(stream) as SlidePuzzleData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + slidePuzzleSavePath);
            return null;
        }
    }
}
