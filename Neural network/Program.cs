using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Neural_network
{
    class Program
    {
        static void Main(string[] args)
        {
            neuron[] inputLayer = new neuron[3]; // these values can be changed to be the parameters for any neural network
            neuron[] hiddenLayer = new neuron[20];
            neuron[] outputLayer = new neuron[9];
            bool direction; // true for forwards, false for backwards
            bool biasDirection;
            float[] outputs = new float[outputLayer.Length];
            float bestCost = 100000000.0f;
            int countWithoutImprovment = 0;
            const int threshold = 5;
            string trainingAndTestingPath = "training.txt";
            // initiaization
            for (int i = 0; i < inputLayer.Length;i++)
            {
                inputLayer[i] = new neuron();
            }
            for (int i = 0; i < hiddenLayer.Length;i++)
            {
                hiddenLayer[i] = new neuron();
            }
            for (int i = 0; i < outputLayer.Length;i++)
            {
                outputLayer[i] = new neuron();
            }
            
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].initialize(1);
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].initialize(inputLayer.Length);
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].initialize(hiddenLayer.Length);
            }

            Console.WriteLine("Training weights");
            // training first line
            string[] trainingData = File.ReadAllLines(trainingAndTestingPath);
            string[] firstLine = trainingData[0].Split(' ');
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].inputs[0] = float.Parse(firstLine[i]);
            }
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].calculate();
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                {
                    hiddenLayer[i].inputs[j] = inputLayer[j].output;
                }
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].calculate();
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                {
                    outputLayer[i].inputs[j] = hiddenLayer[j].output;
                }
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].calculate();
            }
            float[] actualOutput = new float[outputLayer.Length];
            for (int i = 0; i < outputLayer.Length; i++)
            {
                actualOutput[i] = outputLayer[i].output;
            }
            float[] expectedOutput = new float[outputLayer.Length];
            expectedOutput[getExpectedOutput(firstLine[3])] = 1.0f;
            float[] differences = new float[expectedOutput.Length];
            for (int i = 0; i < expectedOutput.Length; i++)
            {
                differences[i] = actualOutput[i] - expectedOutput[i];
            }
            float firstCost = 0.0f;
            for (int i = 0; i < differences.Length; i++)
            {
                firstCost += differences[i];
            }

            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].weightStep(true);
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].weightStep(true);
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].weightStep(true);
            }
            
            // testing second line
            string[] secondLine = trainingData[1].Split(' ');
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].inputs[0] = float.Parse(secondLine[i]);
            }
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].calculate();
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                {
                    hiddenLayer[i].inputs[j] = inputLayer[j].output;
                }
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].calculate();
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                {
                    outputLayer[i].inputs[j] = hiddenLayer[j].output;
                }
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].calculate();
            }
            actualOutput = new float[outputLayer.Length];
            for (int i = 0; i < outputLayer.Length; i++)
            {
                actualOutput[i] = outputLayer[i].output;
            }
            expectedOutput = new float[outputLayer.Length];
            expectedOutput[getExpectedOutput(secondLine[3])] = 1.0f;
            differences = new float[expectedOutput.Length];
            for (int i = 0; i < expectedOutput.Length; i++)
            {
                differences[i] = actualOutput[i] - expectedOutput[i];
            }
            float secondCost = 0.0f;
            for (int i = 0; i < differences.Length; i++)
            {
                secondCost += differences[i];
            }
            
            // figuring out direction
            if (Math.Abs(secondCost) > Math.Abs(firstCost))
            {
                direction = true;
            }
            else
            {
                direction = false;
            }
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].weightStep(direction);
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].weightStep(direction);
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].weightStep(direction);
            }

            // full weight training
            for (int a = 2; a < trainingData.Length-1; a++)
            {
                string[] currLine = trainingData[a].Split(' ');
                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].inputs[0] = float.Parse(currLine[i]);
                }
                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].calculate();
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                    {
                        hiddenLayer[i].inputs[j] = inputLayer[j].output;
                    }
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    hiddenLayer[i].calculate();
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                    {
                        outputLayer[i].inputs[j] = hiddenLayer[j].output;
                    }
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    outputLayer[i].calculate();
                }
                actualOutput = new float[outputLayer.Length];
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    actualOutput[i] = outputLayer[i].output;
                }
                expectedOutput = new float[outputLayer.Length];
                expectedOutput[getExpectedOutput(firstLine[3])] = 1.0f;
                differences = new float[expectedOutput.Length];
                for (int i = 0; i < expectedOutput.Length; i++)
                {
                    differences[i] = actualOutput[i] - expectedOutput[i];
                }
                float cost = 0.0f;
                for (int i = 0; i < differences.Length; i++)
                {
                    cost += differences[i];
                }

                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].weightStep(direction);
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    hiddenLayer[i].weightStep(direction);
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    outputLayer[i].weightStep(direction);
                }
                Console.WriteLine("Current iteration: "+a+" | Current cost: "+cost);
                if (Math.Abs(cost) < Math.Abs(bestCost))
                {
                    bestCost = cost;
                    for (int b = 0; b < inputLayer.Length; b++)
                    {
                        inputLayer[b].bestWeights = inputLayer[b].weights;
                    }
                    for (int b = 0; b < hiddenLayer.Length; b++)
                    {
                        hiddenLayer[b].bestWeights = hiddenLayer[b].weights;
                    }
                    for (int b = 0; b < outputLayer.Length; b++)
                    {
                        outputLayer[b].bestWeights = outputLayer[b].weights;
                    }
                    countWithoutImprovment = 0;
                }
                else 
                {
                    countWithoutImprovment++;
                    if (countWithoutImprovment > threshold)
                    {
                        for (int b = 0; b < inputLayer.Length; b++)
                        {
                            inputLayer[b].weights = inputLayer[b].bestWeights;
                        }
                        for (int b = 0; b < hiddenLayer.Length; b++)
                        {
                            hiddenLayer[b].weights = hiddenLayer[b].bestWeights;
                        }
                        for (int b = 0; b < outputLayer.Length; b++)
                        {
                            outputLayer[b].weights = outputLayer[b].bestWeights;
                        }
                        break;
                    }
                }
            }
            // bias training
            Console.WriteLine("Training biases");
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].bestBias = inputLayer[i].bias;
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].bestBias = hiddenLayer[i].bias;
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].bestBias = outputLayer[i].bias;
            }
            trainingData = File.ReadAllLines(trainingAndTestingPath);
            firstLine = trainingData[0].Split(' ');
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].inputs[0] = float.Parse(firstLine[i]);
            }
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].calculate();
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                {
                    hiddenLayer[i].inputs[j] = inputLayer[j].output;
                }
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].calculate();
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                {
                    outputLayer[i].inputs[j] = hiddenLayer[j].output;
                }
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].calculate();
            }
            actualOutput = new float[outputLayer.Length];
            for (int i = 0; i < outputLayer.Length; i++)
            {
                actualOutput[i] = outputLayer[i].output;
            }
            expectedOutput = new float[outputLayer.Length];
            expectedOutput[getExpectedOutput(firstLine[3])] = 1.0f;
            differences = new float[expectedOutput.Length];
            for (int i = 0; i < expectedOutput.Length; i++)
            {
                differences[i] = actualOutput[i] - expectedOutput[i];
            }
            firstCost = 0.0f;
            for (int i = 0; i < differences.Length; i++)
            {
                firstCost += differences[i];
            }

            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].biasStep(true);
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].biasStep(true);
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].biasStep(true);
            }
            
            // testing second line
            secondLine = trainingData[1].Split(' ');
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].inputs[0] = float.Parse(secondLine[i]);
            }
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].calculate();
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                {
                    hiddenLayer[i].inputs[j] = inputLayer[j].output;
                }
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].calculate();
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                {
                    outputLayer[i].inputs[j] = hiddenLayer[j].output;
                }
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].calculate();
            }
            actualOutput = new float[outputLayer.Length];
            for (int i = 0; i < outputLayer.Length; i++)
            {
                actualOutput[i] = outputLayer[i].output;
            }
            expectedOutput = new float[outputLayer.Length];
            expectedOutput[getExpectedOutput(secondLine[3])] = 1.0f;
            differences = new float[expectedOutput.Length];
            for (int i = 0; i < expectedOutput.Length; i++)
            {
                differences[i] = actualOutput[i] - expectedOutput[i];
            }
            secondCost = 0.0f;
            for (int i = 0; i < differences.Length; i++)
            {
                secondCost += differences[i];
            }
            // figuring out direction
            if (Math.Abs(secondCost) > Math.Abs(firstCost))
            {
                biasDirection = true;
            }
            else
            {
                biasDirection = false;
            }
            for (int i = 0; i < inputLayer.Length; i++)
            {
                inputLayer[i].biasStep(biasDirection);
            }
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i].biasStep(biasDirection);
            }
            for (int i = 0; i < outputLayer.Length; i++)
            {
                outputLayer[i].biasStep(biasDirection);
            }

            // full bias training
            countWithoutImprovment = 0;
            for (int a = 2; a < trainingData.Length-1; a++)
            {
                string[] currLine = trainingData[a].Split(' ');
                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].inputs[0] = float.Parse(currLine[i]);
                }
                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].calculate();
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                    {
                        hiddenLayer[i].inputs[j] = inputLayer[j].output;
                    }
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    hiddenLayer[i].calculate();
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                    {
                        outputLayer[i].inputs[j] = hiddenLayer[j].output;
                    }
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    outputLayer[i].calculate();
                }
                actualOutput = new float[outputLayer.Length];
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    actualOutput[i] = outputLayer[i].output;
                }
                expectedOutput = new float[outputLayer.Length];
                expectedOutput[getExpectedOutput(firstLine[3])] = 1.0f;
                differences = new float[expectedOutput.Length];
                for (int i = 0; i < expectedOutput.Length; i++)
                {
                    differences[i] = actualOutput[i] - expectedOutput[i];
                }
                float cost = 0.0f;
                for (int i = 0; i < differences.Length; i++)
                {
                    cost += differences[i];
                }

                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].biasStep(direction);
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    hiddenLayer[i].biasStep(direction);
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    outputLayer[i].biasStep(direction);
                }
                Console.WriteLine("Current iteration: "+a+" | Current cost: "+cost);
                if (Math.Abs(cost) < Math.Abs(bestCost))
                {
                    bestCost = cost;
                    for (int b = 0; b < inputLayer.Length; b++)
                    {
                        inputLayer[b].bestBias = inputLayer[b].bias;
                    }
                    for (int b = 0; b < hiddenLayer.Length; b++)
                    {
                        hiddenLayer[b].bestBias = hiddenLayer[b].bias;
                    }
                    for (int b = 0; b < outputLayer.Length; b++)
                    {
                        outputLayer[b].bestBias = outputLayer[b].bias;
                    }
                    countWithoutImprovment = 0;
                }
                else 
                {
                    countWithoutImprovment++;
                    if (countWithoutImprovment > threshold)
                    {
                        for (int b = 0; b < inputLayer.Length; b++)
                        {
                            inputLayer[b].bias = inputLayer[b].bestBias;
                        }
                        for (int b = 0; b < hiddenLayer.Length; b++)
                        {
                            hiddenLayer[b].bias = hiddenLayer[b].bestBias;
                        }
                        for (int b = 0; b < outputLayer.Length; b++)
                        {
                            outputLayer[b].bias = outputLayer[b].bestBias;
                        }
                        break;
                    }
                }
            }

            Console.WriteLine("Testing");
            string[]testData = File.ReadAllLines(trainingAndTestingPath);
            for (int a = 0; a < testData.Length; a++)
            {
                string[] currLine = testData[a].Split(' ');
                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].inputs[0] = float.Parse(currLine[i]);
                }
                for (int i = 0; i < inputLayer.Length; i++)
                {
                    inputLayer[i].calculate();
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    for (int j = 0; j < hiddenLayer[i].inputs.Length; j++)
                    {
                        hiddenLayer[i].inputs[j] = inputLayer[j].output;
                    }
                }
                for (int i = 0; i < hiddenLayer.Length; i++)
                {
                    hiddenLayer[i].calculate();
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    for (int j = 0; j < outputLayer[i].inputs.Length; j++)
                    {
                        outputLayer[i].inputs[j] = hiddenLayer[j].output;
                    }
                }
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    outputLayer[i].calculate();
                }
                actualOutput = new float[outputLayer.Length];
                for (int i = 0; i < outputLayer.Length; i++)
                {
                    actualOutput[i] = outputLayer[i].output;
                }
                expectedOutput = new float[outputLayer.Length];
                expectedOutput[getExpectedOutput(currLine[3])] = 1.0f;
                Console.WriteLine("Colour guessed: "+getColourName(actualOutput) + " | Actual colour: " + getColourName(expectedOutput)+" | Confidence: " + actualOutput[getExpectedOutput(currLine[3])]);
            }
        }
        public static int getExpectedOutput(string input)
        {
            string[]colours = {"red","orange","yellow","green","blue","purple","white","grey","black"};
            return Array.IndexOf(colours, input);
        }
        public static string getColourName(float[] input)
        {
            string[]colours = {"red","orange","yellow","green","blue","purple","white","grey","black"};
            return colours[input.ToList().IndexOf(maxVal(input))];
        }
        public static float maxVal(float[] input) // might need to use this, maybe just use the built-in function. I couldn't get the built-in function to work properly
        {
            float maxVal = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] > maxVal)
                {
                    maxVal = input[i];
                }
            }
            return maxVal;
        }
    }
    class neuron
    {
        Random randnum = new Random();
        public float[] inputs;
        public float[] weights;
        float[] weightStepSizes;
        public float[] bestWeights;
        public float output;
        public float bias;
        float biasStepVal;
        public float bestBias;

        public void initialize(int numOfInputs)
        {
            inputs = new float[numOfInputs];
            weights = new float[numOfInputs];
            weightStepSizes = new float[numOfInputs];
            bestWeights = weights;
            bias = (float)randnum.NextDouble() + randnum.Next(-10,10);
            biasStepVal = (float)randnum.NextDouble();
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (float)randnum.NextDouble();
            }
            for (int i = 0; i < weightStepSizes.Length; i++)
            {
                weightStepSizes[i] = (float)randnum.NextDouble();
                if (randnum.Next(0,2) == 0)
                {
                    weightStepSizes[i] *= -1;
                }
            }
        }
        public void calculate()
        {
            float total = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                total += inputs[i] * weights[i];
            }
            total += bias; // comment this out for testing
            output = sigmoid(total);
        }
        public void weightStep(bool direction)
        {
            if (direction == true)
            {
                for (int i = 0; i < weightStepSizes.Length; i++)
                {
                    weights[i] += weightStepSizes[i];
                }
            }
            else
            {
                for (int i = 0; i < weightStepSizes.Length; i++)
                {
                    weights[i] -= weightStepSizes[i];
                }
            }
        }
        public void biasStep(bool direction)
        {
            if (direction == true)
            {
                bias += biasStepVal;
            }
            else
            {
                bias -= biasStepVal;
            }
        }
        public float sigmoid (float input)
        {
            return (float)(1/(1+Math.Exp((double)-input)));
        }
    }
}
