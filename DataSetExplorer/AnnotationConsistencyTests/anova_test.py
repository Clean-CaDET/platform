import sys
import pandas as pd
from statsmodels.formula.api import ols
from statsmodels.stats.anova import anova_lm


if __name__ == '__main__':
    data_file_path = sys.argv[1]
    df = pd.read_excel(data_file_path)
    dependent_variable = sys.argv[2]
    independent_variable = sys.argv[3]
    lm = ols(dependent_variable + '~' + independent_variable, data=df).fit()
    table = anova_lm(lm)
    print("METRIC " + dependent_variable)
    print(table)
