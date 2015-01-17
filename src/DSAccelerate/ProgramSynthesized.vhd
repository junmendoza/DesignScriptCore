library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity ProgramSynthesized is
port( 
	clock : in std_logic;
	reset : in std_logic
);
end ProgramSynthesized;

architecture Behavioral of ProgramSynthesized is

signal execution_started : std_logic;
signal parallel_exec_done_ALU_Add : std_logic;
signal select_index : std_logic_vector(7 downto 0);
signal ALU_Add1_op1 : std_logic_vector(31 downto 0);
signal ALU_Add1_op2 : std_logic_vector(31 downto 0);
signal ALU_Add2_op1 : std_logic_vector(31 downto 0);
signal ALU_Add2_op2 : std_logic_vector(31 downto 0);
signal ALU_Add3_op1 : std_logic_vector(31 downto 0);
signal ALU_Add3_op2 : std_logic_vector(31 downto 0);
signal ALU_Add1_result : std_logic_vector(31 downto 0);
signal ALU_Add2_result : std_logic_vector(31 downto 0);
signal ALU_Add3_result : std_logic_vector(31 downto 0);
type t_array_10_32 is array (0 to 10) of std_logic_vector(31 downto 0);
signal a : t_array_10_32;


signal b : t_array_10_32;


signal c : t_array_10_32;


component Mux_41_Component_In_ALU_Add is
port( 
	reset : in std_logic;
	select_index : in std_logic_vector(7 downto 0);
	op11 : in std_logic_vector(31 downto 0) := (others => '0');
	op21 : in std_logic_vector(31 downto 0) := (others => '0');
	op31 : in std_logic_vector(31 downto 0) := (others => '0');
	op41 : in std_logic_vector(31 downto 0) := (others => '0');
	op12 : in std_logic_vector(31 downto 0) := (others => '0');
	op22 : in std_logic_vector(31 downto 0) := (others => '0');
	op32 : in std_logic_vector(31 downto 0) := (others => '0');
	op42 : in std_logic_vector(31 downto 0) := (others => '0');
	op1 : out std_logic_vector(31 downto 0);
	op2 : out std_logic_vector(31 downto 0)
);
end component Mux_41_Component_In_ALU_Add;

component ALU_Add is
port( 
	reset : in std_logic;
	op1 : in std_logic_vector(31 downto 0);
	op2 : in std_logic_vector(31 downto 0);
	result : out std_logic_vector(31 downto 0)
);
end component ALU_Add;


begin
call_1_Mux_41_Component_In_ALU_Add : Mux_41_Component_In_ALU_Add port map
(
reset => reset,
select_index => select_index,
op11 => a(0),
op21 => a(3),
op31 => a(6),
op41 => a(9),
op12 => b(0),
op22 => b(3),
op32 => b(6),
op42 => b(9),
op1 => ALU_Add1_op1,
op2 => ALU_Add1_op2
);
call_2_Mux_41_Component_In_ALU_Add : Mux_41_Component_In_ALU_Add port map
(
reset => reset,
select_index => select_index,
op11 => a(1),
op21 => a(4),
op31 => a(7),
op41 => open,
op12 => b(1),
op22 => b(4),
op32 => b(7),
op42 => open,
op1 => ALU_Add2_op1,
op2 => ALU_Add2_op2
);
call_3_Mux_41_Component_In_ALU_Add : Mux_41_Component_In_ALU_Add port map
(
reset => reset,
select_index => select_index,
op11 => a(2),
op21 => a(5),
op31 => a(8),
op41 => open,
op12 => b(2),
op22 => b(5),
op32 => b(8),
op42 => open,
op1 => ALU_Add3_op1,
op2 => ALU_Add3_op2
);
call_1_ALU_Add : ALU_Add port map
(
reset => reset,
op1 => ALU_Add1_op1,
op2 => ALU_Add1_op2,
result => ALU_Add1_result
);
call_2_ALU_Add : ALU_Add port map
(
reset => reset,
op1 => ALU_Add2_op1,
op2 => ALU_Add2_op2,
result => ALU_Add2_result
);
call_3_ALU_Add : ALU_Add port map
(
reset => reset,
op1 => ALU_Add3_op1,
op2 => ALU_Add3_op2,
result => ALU_Add3_result
);

proc_1_ProgramSynthesized : process(clock)

begin
ResetSync : if reset = '1' then
execution_started <= '0';

elsif reset = '0' then
ClockSync : if rising_edge(clock) then
if execution_started = '0' then
execution_started <= '1';
a(0) <= X"00000001";
a(1) <= X"00000002";
a(2) <= X"00000003";
a(3) <= X"00000004";
a(4) <= X"00000005";
a(5) <= X"00000006";
a(6) <= X"00000007";
a(7) <= X"00000008";
a(8) <= X"00000009";
a(9) <= X"0000000A";
b(0) <= X"0000000A";
b(1) <= X"00000014";
b(2) <= X"0000001E";
b(3) <= X"00000028";
b(4) <= X"00000032";
b(5) <= X"0000003C";
b(6) <= X"00000046";
b(7) <= X"00000050";
b(8) <= X"0000005A";
b(9) <= X"00000064";

end if ;

end if ClockSync;

end if ResetSync;


end process proc_1_ProgramSynthesized;

proc_2_WriteBackControlUnit : process(ALU_Add1_result, ALU_Add2_result, ALU_Add3_result)
variable iterationCount: integer;

begin
ResetSync : if reset = '1' then
select_index <= X"00";

elsif reset = '0' then
iterationCount := to_integer(signed(select_index));
iterationCount := iterationCount + 1;
select_index <= std_logic_vector(to_signed(iterationCount, 8));
if select_index = X"00" then
c(0) <= ALU_Add1_result;
c(1) <= ALU_Add2_result;
c(2) <= ALU_Add3_result;

elsif select_index = X"01" then
c(3) <= ALU_Add1_result;
c(4) <= ALU_Add2_result;
c(5) <= ALU_Add3_result;

elsif select_index = X"02" then
c(6) <= ALU_Add1_result;

end if ;

end if ResetSync;


end process proc_2_WriteBackControlUnit;

proc_3_IterationControlUnit : process(reset, select_index)

begin
ResetSync : if reset = '1' then
parallel_exec_done_ALU_Add <= '0';

elsif reset = '0' then
if parallel_exec_done_ALU_Add = '0' then
if select_index = X"05" then
parallel_exec_done_ALU_Add <= '1';

end if ;

end if ;

end if ResetSync;


end process proc_3_IterationControlUnit;


end Behavioral;
