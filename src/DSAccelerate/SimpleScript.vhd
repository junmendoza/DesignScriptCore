----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    22:12:33 12/29/2014 
-- Design Name: 
-- Module Name:    SimpleScript - Behavioral 
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
--library UNISIM;
--use UNISIM.VComponents.all;

-- def Increment(a : int)
-- {
--		t = a + 1;
--		return = t;
-- }
-- x = 1;
-- y = Increment(x);
-- z = y + 2;

entity SimpleScript is
	Port( 
			clock : in STD_LOGIC;
		   reset : in STD_LOGIC
		 );
end SimpleScript;

architecture Behavioral of SimpleScript is

	component Increment is
		Port( 
				reset : in STD_LOGIC;
				a : in STD_LOGIC_VECTOR(31 downto 0);
				return_val : out STD_LOGIC_VECTOR(31 downto 0)
			 );
	end component Increment;
	
	component ALU_Add is
		Port( 
				reset : in STD_LOGIC;
				op1 : in STD_LOGIC_VECTOR (31 downto 0);
				op2 : in STD_LOGIC_VECTOR (31 downto 0);
				result : out  STD_LOGIC_VECTOR (31 downto 0)
			  );
	end component ALU_Add;

	signal execution_started : STD_LOGIC;
	signal Increment_return : STD_LOGIC_VECTOR(31 downto 0);

	signal x : STD_LOGIC_VECTOR(31 downto 0);
	signal y : STD_LOGIC_VECTOR(31 downto 0);
	signal z : STD_LOGIC_VECTOR(31 downto 0);
	
	signal Increment_call_1_return_val : STD_LOGIC_VECTOR(31 downto 0);
	signal ALU1_Add_return : STD_LOGIC_VECTOR(31 downto 0);

begin

	Increment_call_1 : Increment port map
	(
		reset => reset,
		a => x,
		return_val => Increment_call_1_return_val
	);
		

	ALU1_Add : ALU_Add port map
	(
		reset => reset,
		op1 => y,
		op2 => X"00000002",
		result => ALU1_Add_return
	);
	
	-- entry process
	proc1_SimpleScript : process(clock)
	begin
		ResetSync : if reset = '1' then
			execution_started <= '0';
		elsif reset = '0' then
			ClockSync : if rising_edge(clock) then
				if execution_started = '0' then
					execution_started <= '1';
					
					x <= X"00000001"; -- 1
					
				end if; 
			end if ClockSync;
		end if ResetSync;
	end process proc1_SimpleScript;


	proc2_return_Increment_call_1 : process(Increment_call_1_return_val)
	begin
		ResetSync : if reset = '1' then
		
		elsif reset = '0' then
			y <= Increment_call_1_return_val;
					
		end if ResetSync;
	end process proc2_return_Increment_call_1;
	
	
	proc3_return_ALU_Add : process(ALU1_Add_return)
	begin
		ResetSync : if reset = '1' then
		
		elsif reset = '0' then
			z <= ALU1_Add_return;
					
		end if ResetSync;
	end process proc3_return_ALU_Add;

end Behavioral;

