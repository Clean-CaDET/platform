<p align="center">
  
  ![Cover](https://raw.githubusercontent.com/wiki/Clean-CaDET/platform/images/overview/cover.jpg)
  
</p>

<h1 align="center">DataSet Explorer</h1>
<div align="center">

  [![CodeFactor](https://www.codefactor.io/repository/github/clean-cadet/dataset-explorer/badge)](https://www.codefactor.io/repository/github/clean-cadet/dataset-explorer)
  [![Gitter](https://badges.gitter.im/Clean-CaDET/community.svg)](https://gitter.im/Clean-CaDET/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

</div>

<p align="justify">
  The DataSet Explorer (DSE) tool supports annotators during the code smell annotation procedure.
  </p>
<p align="justify">
  DSE tool development started as a part of the Clean CaDET project which was funded by the <a href="http://fondzanauku.gov.rs/?lang=en">Science Fund of the Republic of Serbia</a>.
</p>

- [Motivation](#motivation)
- [Who is it for?](#who-is-it-for)
- [Get started](#get-started)
- [Useful resources](#useful-resources)
- [Team](#team)

# Motivation
<p align="justify">
Maintainability is an aspect of software quality that refers to the ease with which software can be modified to correct faults, improve performance, or adjust to a new environment. Software maintainability can be negatively impacted by code smells, which are structures in code that indicate issues in software design or implementation. Software engineering experts agree that detecting and removing harmful code smells is important for high-quality code. Machine learning (ML) models could be used to detect code smells, but the models must be trained on high-quality datasets to be accurate and useful to software engineers. 
</p>
<p align="justify">
  Datasets created automatically using heuristic-based tools can result in false positives and false negatives. Semi-automated approaches require experts to validate annotations made by tool, but these datasets may still contain false negatives. On the other hand, fully manual approach is challenging. Inconsistent annotations, small size, non-realistic smell-to-non-smell ratio, and poor smell coverage hinder the dataset quality. These issues arise mainly due to the time-consuming nature of manual annotation and annotators' disagreements caused by ambiguous and vague smell definitions.
</p>
<p align="justify">
  To speed up and ease the manual code smell annotation, we developed the DataSet Explorer (DSE) tool. This tool supports annotators during the annotation procedure by providing various functionalities described in detail <a href="https://github.com/Clean-CaDET/dataset-explorer/wiki/Module-Dataset-Explorer" target="_blank">here</a>.
</p>

# Who is it for?
<p align="justify">
  The DSE tool can be used by annotators and ML researchers aiming to build high-quality datasets which can be used to train ML code smell detection models.
</p>

# Get started
Set up and get started with DSE tool by following these <a href="https://github.com/Clean-CaDET/dataset-explorer/blob/master/SETUP.md" target="_blank">instructions</a>.

# Useful resources
We outline notable resources that can assist researchers in using our implementation:
<ul>
  <li><a href="https://github.com/Clean-CaDET/dataset-explorer" target="_blank">Back-end source code</a> - A repository hosting the source code of the DSE server application</li>
  <li><a href="https://github.com/Clean-CaDET/platform-explorer-ui-web" target="_blank">Front-end source code</a> - A repository hosting the source code of the web UI</li>
  <li><a href="https://github.com/Clean-CaDET/dataset-explorer/wiki" target="_blank">General documentation</a> - Wiki pages explaining our DSE design and supported features</li>
</ul>

# Team
<p align="justify">
  Our project team consists of professors and teaching assistants from the Faculty of Technical Sciences, Novi Sad, Serbia. We are part of the Chair of Informatics, an organizational unit that has traditionally been the local center of excellence for both artificial intelligence and software engineering research.
</p>

- The people that make up the Clean CaDET Core are listed [here](https://clean-cadet.github.io/about/).
