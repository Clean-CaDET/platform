<p align="center">
  
  ![Cover](https://raw.githubusercontent.com/wiki/Clean-CaDET/platform/images/overview/cover.jpg)
  
</p>

<h1 align="center">Clean CaDET</h1>
<div align="center">

  [![CodeFactor](https://www.codefactor.io/repository/github/clean-cadet/platform/badge)](https://www.codefactor.io/repository/github/clean-cadet/platform)
  [![Gitter](https://badges.gitter.im/Clean-CaDET/community.svg)](https://gitter.im/Clean-CaDET/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

</div>

<p align="justify">
  The Clean Code and Design Educational Tool (Clean CaDET) is a platform dedicated to the study of clean code. It presents a conglomerate of AI-powered tools for educators, learners, practitioners, and researchers studying clean code.
  </p>
<p align="justify">
  Clean CaDET started as a project funded by the <a href="http://fondzanauku.gov.rs/?lang=en">Science Fund of the Republic of Serbia</a>. It hopes to grow into an active open-source community dedicated to software engineers' growth and their pursuit to build sustainable, high-quality software.
</p>

- [Introduction](#introduction)
  - [What is the problem?](#what-is-the-problem)
  - [Who is it for?](#who-is-it-for)
- [Get started](#get-started)
  - [Requirements](#requirements)
  - [Packages](#packages)
- [Team](#team)

# Introduction
The vision and high-level idea behind Clean CaDET is described in the [overview video](https://www.youtube.com/watch?v=fBENFfjC49A). 

## What is the problem?
<p align="justify">
  There is a lot of flexibility when crafting software solutions, especially those at a higher level of abstraction. Software engineers have a vast pool of tools and technologies to choose from when assembling contemporary software. This flexibility has an interesting consequence – a requirement can be fulfilled by a near-infinite set of different code configurations. Even when limited to a single programming language and a simple requirement, it is easy to list many code samples that fulfill the requirement using different coding styles and language features.
</p>
<p align="justify">
  While many code solutions can fulfill a requirement, not all of them are acceptable. Some solutions cause subtle bugs, performance loss, or expose security vulnerabilities. Furthermore, many of the possible solutions present another severe but less obvious problem in the form of code smells. Code suffering from sever code smells is hard to understand and modify. Such code harms the software’s maintainability, evolvability, reliability, and testability, introducing technical debt. Unfortunately, removing code smells is not easy, as many code smell definitions are vague and lack a concrete heuristic that can unambiguously determine the smell’s presence.
</p>

## Who is it for?

### Practitioners
Through its primary feature set, Clean CaDET detects code smells through AI models. It then offers personalized suggestions to the user in the form of educational content to help them resolve the identified issues. It acts as a digital assistant for software engineering practitioners, which integrates into their development environment to analyze their code.

- For more details regarding the **code quality analysis** workflow, useful for *practitioners*, check out the [wiki pages](https://github.com/Clean-CaDET/platform/wiki).

### Learners and educators
A significant module of Clean CaDET is the *Smart Tutor*. It hosts the learner's model, a collection of learning objects, and instructional rules that select the most appropriate educational content for the particular learner. This functionality is integrated into the code quality analysis workflow, and it can be accessed as a standalone educational tool. By directly interacting with the *Smart Tutor*, learners can explore various clean code topics and engage with the challenge subsystem to learn how to refactor and analyze code quality in a gamified environment.

- For more details regarding the **Smart Tutor** module, useful for *learners and educators*, check out the [module's page](https://github.com/Clean-CaDET/platform/wiki/Module-Smart-Tutor).

### Researchers
While developing AI algorithms for code smell detection, we processed existing datasets and built [our own](https://www.techrxiv.org/articles/preprint/Towards_a_systematic_approach_to_manual_annotation_of_code_smells/14159183). To automate the process of dataset construction and analysis, we developed the *Dataset Explorer* tool.

- For more details regarding the **Dataset Explorer** module, useful for *researchers*, check out the [module's page](https://github.com/Clean-CaDET/platform/wiki/Module-Dataset-Explorer).

# Team
<p align="justify">
  Our project team consists of professors and teaching assistants from the Faculty of Technical Sciences, Novi Sad, Serbia. We are part of the Chair of Informatics, an organizational unit that has traditionally been the local center of excellence for both artificial intelligence and software engineering research.
</p>

- The people that make up the Clean CaDET Core are listed [here](https://clean-cadet.github.io/about/).
