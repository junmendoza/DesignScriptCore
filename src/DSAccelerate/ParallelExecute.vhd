----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    12:28:00 12/08/2014 
-- Design Name: 
-- Module Name:    ParallelExecute - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
library UNISIM;
use UNISIM.VComponents.all;

-------------------------------
--	Parallel execution
--
--	a : int[] = {1,2,3}
--	b : int[] = {4,5,6}
--	c : int[] = a + b
--
-- a = allocate_array(3)
-- b = allocate_array(3)
-- c = allocate_array(3)
-- Serial initialize
-- a[0] = 1	 
-- a[1] = 2
-- a[2] = 3
-- b[0] = 4
-- b[1] = 5
-- b[2] = 6
--- Parallel execute
-- c [0] = a[0] + b[0]
-- c [1] = a[1] + b[1]
-- c [2] = a[2] + b[2]

-------------------------------

entity ParallelExecute is
	Port( 
			clock : in STD_LOGIC;
			reset : in STD_LOGIC
		 );
end ParallelExecute;

architecture Behavioral of ParallelExecute is

	component ALU_Add is
		Port( 
				reset : in STD_LOGIC;
				op1 : in STD_LOGIC_VECTOR (31 downto 0);
				op2 : in STD_LOGIC_VECTOR (31 downto 0);
				result : out  STD_LOGIC_VECTOR (31 downto 0)
			  );
	end component ALU_Add;

	type t_static_array_decl is array (0 to 2) of STD_LOGIC_VECTOR(31 downto 0);

	signal array_a : t_static_array_decl :=
	(
		X"00000001", 
		X"00000002", 
		X"00000003"
	);
	
	signal array_b : t_static_array_decl :=
	(
		X"00000004", 
		X"00000005", 
		X"00000006"
	);
	
	signal array_c : t_static_array_decl :=
	(
		X"00000000", 
		X"00000000", 
		X"00000000"
	);
	
begin

	Add0 : ALU_Add port map
	(
		reset => reset,
		op1 => array_a(0),
		op2 => array_b(0),
		result => array_c(0)
	);
	
	Add1 : ALU_Add port map
	(
		reset => reset,
		op1 => array_a(1),
		op2 => array_b(1),
		result => array_c(1)
	);
	
	Add2 : ALU_Add port map
	(
		reset => reset,
		op1 => array_a(2),
		op2 => array_b(2),
		result => array_c(2)
	);
	

end Behavioral;

