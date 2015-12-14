#Compatibility Test Case

##Definition
Compatible: API Container Decalaration is compatible as with as all child APIs are compatible.

##Interface
Case|Compile Compatible|Runtime Compatible
----|------------------|------------------
Method Added|Broken|Broken
Generic Param name changed|True|True
*Generic constraint violated*|Broken|Broken
Inherited interface added|Broken|Broken
Inherited interface removed|True|True

##Member
Case|Compile Compatible|Runtime Compatible
----|------------------|------------------
protected virtual->public|Broken|Broken
private->public|True|True
protect->public|True|True
Generic Param name changed|True|True
*Generic constraint violated*|Broken|Broken
virtual removed|Broken|True

##Enum
Case|Compile Compatible|Runtime Compatible
----|------------------|------------------
member order changed|True|True
Flags Attribute removed|True|True

##Abstract
Case|Compile Compatible|Runtime Compatible
----|------------------|------------------
abstract removed|Broken|True