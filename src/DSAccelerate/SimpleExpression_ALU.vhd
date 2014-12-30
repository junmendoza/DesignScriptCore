----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    12:40:20 12/06/2014 
-- Design Name: 
-- Module Name:    SimpleExpressionALU - Behavioral 
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
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;


---------------------------
-- SimpleExpressionALU
-- 	a : int = 1
-- 	b : int = a + 2
--
--		allocate(a)
--		allocate(b)
--		a = 1
--		b = a + 2
---------------------------
entity SimpleExpressionALU is
	Port( 
			clock : in  STD_LOGIC;
			reset : in  STD_LOGIC;
			
			-- in arg list
		
			-- out return list
			a_out : out STD_LOGIC_VECTOR(31 downto 0);
			b_out : out STD_LOGIC_VECTOR(31 downto 0)
	  );
end SimpleExpressionALU;

architecture Behavioral of SimpleExpressionALU is

	component ALU_Add is
		Port( 
				reset : in STD_LOGIC;
				op1 : in  STD_LOGIC_VECTOR (31 downto 0);
				op2 : in  STD_LOGIC_VECTOR (31 downto 0);
				result : out  STD_LOGIC_VECTOR (31 downto 0)
			  );
	end component ALU_Add;

	signal a : STD_LOGIC_VECTOR(31 downto 0);
	signal b : STD_LOGIC_VECTOR(31 downto 0);
	signal t0 : STD_LOGIC_VECTOR(31 downto 0);
	signal t1 : STD_LOGIC_VECTOR(31 downto 0);
	
	-- Flag to execute once
	signal exec : STD_LOGIC;

begin

	AddOperands : ALU_Add port map
	(
		reset => reset,
		op1 => a,
		op2 => t0,
		result => t1
	);

	ProcExecute : process(clock)
	
	begin 
		ResetSync : if reset = '1' then
			exec <= '1';
		elsif reset = '0' then
			ClockSync : if rising_edge(clock) then
				if exec = '1' then
					exec <= '0';
					a <= X"00000001";
					t0 <= X"00000002";
				end if;
			end if ClockSync;
		end if ResetSync;
	end process ProcExecute;
	
	ProcReturnResult : process(t1)
	begin 
		ResetSync : if reset = '0' then
			a_out <= a;
			b_out <= t1;
		end if ResetSync;
	end process ProcReturnResult;

end Behavioral;

