import sys
import pandas as pd
from statsmodels.multivariate.manova import MANOVA


if __name__ == '__main__':
    data_file_path = sys.argv[1]
    df = pd.read_excel(data_file_path)
    dependent_variables = sys.argv[2]
    independent_variable = sys.argv[3]
    maov = MANOVA.from_formula(dependent_variables + '~' + independent_variable, data=df)
    print(maov.mv_test())
