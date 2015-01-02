library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Increment is
port( 
	reset : in std_logic;
	i : in std_logic_vector(31 downto 0);
	return_Increment : out std_logic_vector(31 downto 0)
);
end Increment;

architecture Behavioral of Increment is

signal call_2_ALU_Add_return_val : std_logic_vector(31 downto 0);
signal tSSA_1_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_2_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal val : std_logic_vector(31 downto 0);
signal tSSA_3_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);

component ALU_Add is
port( 
	reset : in std_logic;
	op1 : in std_logic_vector(31 downto 0);
	op2 : in std_logic_vector(31 downto 0);
	result : out std_logic_vector(31 downto 0)
);
end component ALU_Add;

begin
call_2_ALU_Add : ALU_Add port map
(
reset => reset,
op1 => tSSA_1_4cc42779139d4e269b03bb7d3686bb3c,
op2 => X"00000001",
result => call_2_ALU_Add_return_val
);

proc_1_Increment : process(i)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_1_4cc42779139d4e269b03bb7d3686bb3c <= i;
end if ResetSync;


end process proc_1_Increment;

proc_2_call_2_ALU_Add_return_val : process(call_2_ALU_Add_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_2_4cc42779139d4e269b03bb7d3686bb3c <= call_2_ALU_Add_return_val;
end if ResetSync;


end process proc_2_call_2_ALU_Add_return_val;

proc_3_tSSA_2_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_2_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
val <= tSSA_2_4cc42779139d4e269b03bb7d3686bb3c;
end if ResetSync;


end process proc_3_tSSA_2_4cc42779139d4e269b03bb7d3686bb3c;

proc_4_val : process(val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_3_4cc42779139d4e269b03bb7d3686bb3c <= val;
end if ResetSync;


end process proc_4_val;

proc_5_tSSA_3_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_3_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
return_Increment <= tSSA_3_4cc42779139d4e269b03bb7d3686bb3c;
end if ResetSync;


end process proc_5_tSSA_3_4cc42779139d4e269b03bb7d3686bb3c;


end Behavioral;
