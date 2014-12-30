----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    01:20:01 12/22/2014 
-- Design Name: 
-- Module Name:    PartialLoopUnrolling - Behavioral 
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


-- Add elements from a to b
-- a : int[] = {1, 2, 3, 4, 5, 6, 7}
-- b : int[] = {10, 20, 30, 40, 50, 60, 70}
-- c : int[] = {}
-- for (int i = 0; i < 7; ++i) {
--		c[i] = a[i] + b[i]
-- }
--
-- Semantics
--
-- a = allocate_array(7)
-- b = allocate_array(7)
-- c = allocate_array(7)
-- a = {1, 2, 3, 4, 5, 6, 7}
-- b = {10, 20, 30, 40, 50, 60, 70}
-- i = 2
-- n = floor(7/i) = 3
-- last = 7 mod 2 = 1
-- cnt = 7 - 1 = 6
-- for (int i = 0; i < cnt; i += n) 
-- {
--		c[i] = a[i] + b[i]
--		c[i+1] = a[i+1] + b[i+1]	
--		c[i+2] = a[i+2] + b[i+2]
--		cnt = cnt + n
-- }
--
-- for (int i = 0; i < last; i += n)
-- { 
--		c[i] = a[i] + b[i]
--		c[i] = a[i] + b[i]
-- }


entity PartialLoopUnrolling is
	Port( 
			clock : in STD_LOGIC;
			reset : in STD_LOGIC
		 );
end PartialLoopUnrolling;

architecture Behavioral of PartialLoopUnrolling is

	component ALU_Add is
		Port( 
				reset : in STD_LOGIC;
				op1 : in STD_LOGIC_VECTOR (31 downto 0);
				op2 : in STD_LOGIC_VECTOR (31 downto 0);
				result : out  STD_LOGIC_VECTOR (31 downto 0)
			  );
	end component ALU_Add;
	
	component Mux31_ALU_In is
	Port( 
			reset : in STD_LOGIC;
			select_index : in STD_LOGIC_VECTOR (7 downto 0);
			
			-- Inputs to ALU op1
			op11 : in STD_LOGIC_VECTOR (31 downto 0);
			op21 : in STD_LOGIC_VECTOR (31 downto 0);
			op31 : in STD_LOGIC_VECTOR (31 downto 0) := (others => '0');
			
			-- Inputs to ALU op2
			op12 : in STD_LOGIC_VECTOR (31 downto 0);
			op22 : in STD_LOGIC_VECTOR (31 downto 0);
			op32 : in STD_LOGIC_VECTOR (31 downto 0) := (others => '0');
			
			-- ALU inputs
			op1 : out STD_LOGIC_VECTOR (31 downto 0);
			op2 : out STD_LOGIC_VECTOR (31 downto 0)
		 );
	end component Mux31_ALU_In;

	type t_static_array_decl is array (0 to 6) of STD_LOGIC_VECTOR(31 downto 0);

-- Array contents 
--	signal array_a : t_static_array_decl :=
--	(
--		X"00000001", 	-- 1
--		X"00000002", 	-- 2
--		X"00000003", 	-- 3
--		X"00000004", 	-- 4
--		X"00000005", 	-- 5
--		X"00000006", 	-- 6
--		X"00000007"  	-- 7
--	);
--	
--	signal array_b : t_static_array_decl :=
--	(
--		X"0000000A",	-- 10 
--		X"00000014",	-- 20 
--		X"0000001E",	-- 30
--		X"00000028", 	-- 40
--		X"00000032", 	-- 50
--		X"0000003C",	-- 60
--		X"00000046"		-- 70
--	);

-- Array contents
-- Testing process execution for unmodified operands
	signal array_a : t_static_array_decl :=
	(
		X"00000001", 	-- 1
		X"00000001", 	-- 1
		X"00000001", 	-- 1
		X"00000001", 	-- 1
		X"00000001", 	-- 1
		X"00000001", 	-- 1
		X"00000001"  	-- 1
	);
	
	signal array_b : t_static_array_decl :=
	(
		X"00000001",	-- 1 
		X"00000001",	-- 1
		X"00000001",	-- 1
		X"00000001", 	-- 1
		X"00000001", 	-- 1
		X"00000001",	-- 1
		X"00000001"		-- 1
	);
	
	signal array_c : t_static_array_decl :=
	(
		X"00000000", 
		X"00000000", 
		X"00000000", 
		X"00000000", 
		X"00000000", 
		X"00000000", 
		X"00000000" 
	);
	
	-- Execution flags
	signal exec_complete : STD_LOGIC := '0'; 
	signal loop_complete : STD_LOGIC := '0'; 
	
	constant i : integer := 1;
	signal select_index : STD_LOGIC_VECTOR(7 downto 0); 
	
	-- ALU in
	signal ALU1_op1 : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU1_op2 : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU2_op1 : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU2_op2 : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU3_op1 : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU3_op2 : STD_LOGIC_VECTOR(31 downto 0); 
	
	-- ALU out
	signal ALU1_result : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU2_result : STD_LOGIC_VECTOR(31 downto 0); 
	signal ALU3_result : STD_LOGIC_VECTOR(31 downto 0); 
	
begin

	---------------------------
	-- ALU input multiplexers
	---------------------------
	Mux_ALU1_in : Mux31_ALU_In port map
	(
		reset => reset,
		select_index => select_index,
		op11	=> array_a(0),
		op21	=> array_a(3),
		op31	=> array_a(6),
		op12	=> array_b(0),
		op22	=> array_b(3),
		op32	=> array_b(6),
		op1 	=> ALU1_op1,
		op2 	=> ALU1_op2
	);
	
	Mux_ALU2_in : Mux31_ALU_In port map
	(
		reset => reset,
		select_index => select_index,
		op11	=> array_a(1),
		op21	=> array_a(4),
		op31	=> open,
		op12	=> array_b(1),
		op22	=> array_b(4),
		op32	=> open,
		op1 	=> ALU2_op1,
		op2 	=> ALU2_op2
	);
	
	Mux_ALU3_in : Mux31_ALU_In port map
	(
		reset => reset,
		select_index => select_index,
		op11	=> array_a(2),
		op21	=> array_a(5),
		op31	=> open,
		op12	=> array_b(2),
		op22	=> array_b(5),
		op32	=> open,
		op1 	=> ALU3_op1,
		op2 	=> ALU3_op2
	);

	---------------------------
	-- ALU 
	---------------------------
	ALU1_Add : ALU_Add port map
	(
		reset => reset,
		op1 => ALU1_op1,
		op2 => ALU1_op2,
		result => ALU1_result
	);
	
	ALU2_Add : ALU_Add port map
	(
		reset => reset,
		op1 => ALU2_op1,
		op2 => ALU2_op2,
		result => ALU2_result
	);
	
	ALU3_Add : ALU_Add port map
	(
		reset => reset,
		op1 => ALU3_op1,
		op2 => ALU3_op2,
		result => ALU3_result
	);
	
	Writeback_ALU_result : process(ALU1_result, ALU2_result, ALU3_result)
		variable iterationCount : integer;
	begin
		ResetSync : if reset = '1' then
			select_index <= X"00";
			
		elsif reset = '0' then
			-- Update ther iteration index
			iterationCount := to_integer(signed(select_index));
			iterationCount := iterationCount + 1;
			select_index <= std_logic_vector(to_signed(iterationCount, 8));
			
			-- Writeback to array
			if select_index = X"00" then
				array_c(0) <= ALU1_result;
				array_c(1) <= ALU2_result;
				array_c(2) <= ALU3_result;
			elsif select_index = X"01" then
				array_c(3) <= ALU1_result;
				array_c(4) <= ALU2_result;
				array_c(5) <= ALU3_result;
			elsif select_index = X"02" then
				array_c(5) <= ALU1_result;
			end if;
		end if ResetSync;
	end process Writeback_ALU_result;
	
	IterationControlUnit : process(reset, select_index)
	begin
		ResetSync : if reset = '1' then
			loop_complete <= '0';
		elsif reset = '0' then
			IsExecutionDone : if loop_complete = '0' then
				if select_index = X"03" then
					loop_complete <= '1';
				end if;
			end if IsExecutionDone;
		end if ResetSync;
	end process IterationControlUnit;
	
	-- ExecutionControlUnit will wait for all Execution Units to complete and then flag exec_done
	ExecutionControlUnit : process(reset, loop_complete)
	begin
		ResetSync : if reset = '1' then
			exec_complete <= '0';
		elsif reset = '0' then
			IsExecutionDone : if exec_complete = '0' then
				-- Check for all exeution unit completion
				if loop_complete = '1' then
					exec_complete <= '1';
				end if;
			end if IsExecutionDone;
		end if ResetSync;
	end process ExecutionControlUnit;

end Behavioral;

