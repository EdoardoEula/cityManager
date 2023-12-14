import argparse
import joblib
import sys

# Load the random forest model from the joblib file
random_forest_model = joblib.load("random_forest_model.joblib")

# Define your functions here
def predict(input_features):
    # Make sure to arrange the input features in the same order as the model expects
    # For example: [age, education, q1, q2, q3, gender_Female, gender_Male, gender_Non-binary, gender_Prefer not to say]
    prediction = random_forest_model.predict([input_features])
    return f"Prediction result: {prediction}"

print('starting')

# Argument parsing
parser = argparse.ArgumentParser(description='Python script for model prediction')
parser.add_argument('--age', type=float, help='Age')
parser.add_argument('--education', type=float, help='Education level')
parser.add_argument('--q1', type=float, help='Q1 value')
parser.add_argument('--q2', type=float, help='Q2 value')
parser.add_argument('--q3', type=float, help='Q3 value')
parser.add_argument('--gender_Female', type=int, help='1 if female else 0')
parser.add_argument('--gender_Male', type=int, help='1 if male else 0')
parser.add_argument('--gender_Non-binary', type=int, help='1 if non-binary else 0')
parser.add_argument('--gender_Prefer_not_to_say', type=int, help='1 if prefer not to say else 0')

args = parser.parse_args()

# Gather input features from command line arguments
input_features = [
    args.age, args.education, args.q1, args.q2, args.q3,
    args.gender_Female, args.gender_Male, args.gender_Non_binary, args.gender_Prefer_not_to_say
]

# Call the predict function with the input features
result = predict(input_features)

print(result)

sys.stdout.write(result)

