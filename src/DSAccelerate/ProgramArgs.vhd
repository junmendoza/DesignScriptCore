----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    12:11:12 12/09/2014 
-- Design Name: 
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
-- Module Name:    ProgramArgs - Behavioral 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
library UNISIM;
use UNISIM.VComponents.all;

use work.Definitions.all;

------------------------
-- a = x
-- b = y
-- c = a + b
--
-- allocate(x)
-- allocate(y)
-- allocate(a)
-- allocate(b)
-- allocate(c)
-- x = x_in
-- y = y_in
-- a = x
-- b = y
-- c = a + b
------------------------
entity ProgramArgs is
	Port( 
			clock 	: in STD_LOGIC;
			reset	 	: in STD_LOGIC;
			sw1	 	: in STD_LOGIC;
			sw2	 	: in STD_LOGIC;
			sw3	 	: in STD_LOGIC;
			a_out 	: out STD_LOGIC_VECTOR (31 downto 0);
			b_out 	: out STD_LOGIC_VECTOR (31 downto 0);
			c_out 	: out STD_LOGIC_VECTOR (31 downto 0);
			LCD_E 	: out STD_LOGIC;
			LCD_RS 	: out STD_LOGIC;
			LCD_RW	: out STD_LOGIC;
			LCD_DB	: out STD_LOGIC_VECTOR(7 downto 0);
			LED 		: out STD_LOGIC_VECTOR(7 downto 0)
		  );
end ProgramArgs;

architecture Behavioral of ProgramArgs is

	component EmitLCD is
		Port( 
				clock 		: in STD_LOGIC;
				reset 		: in STD_LOGIC; 
				char_array	: in STD_LOGIC_VECTOR(79 downto 0);		
				LCDDataBus	: out STD_LOGIC_VECTOR(7 downto 0); 
				LCD_E			: out STD_LOGIC;
				LCD_RS		: out STD_LOGIC;
				LCD_RW		: out STD_LOGIC
			 );
	end component EmitLCD;
	
	component DecodeDisplayString is
		Port( 
				reset : in  STD_LOGIC;
				exec_state : in EXECUTION_STATE;
				var_index : in  STD_LOGIC_VECTOR (2 downto 0);
				char_array : out  STD_LOGIC_VECTOR (79 downto 0)
			 );
	end component DecodeDisplayString;

	component ALU_Add is
		Port( 
				reset : in STD_LOGIC;
				op1 : in STD_LOGIC_VECTOR (31 downto 0);
				op2 : in STD_LOGIC_VECTOR (31 downto 0);
				result : out  STD_LOGIC_VECTOR (31 downto 0)
			  );
	end component ALU_Add;
	
	-- program input signals
	signal input_set : STD_LOGIC_VECTOR (2 downto 0) := (others => '0');

	signal x : STD_LOGIC_VECTOR (31 downto 0);
	signal y : STD_LOGIC_VECTOR (31 downto 0);
	signal a : STD_LOGIC_VECTOR (31 downto 0);
	signal b : STD_LOGIC_VECTOR (31 downto 0);
	signal c : STD_LOGIC_VECTOR (31 downto 0);

	-- LCD view
	-- index into which variable to preview
	signal var_index: STD_LOGIC_VECTOR (2 downto 0) := (others => '0');
	signal char_array: STD_LOGIC_VECTOR (79 downto 0) := (others => '0');
	
	type t_stringtable is array (0 to 2) of STD_LOGIC_VECTOR(79 downto 0);
	signal stringtable : t_stringtable;
	
	
	-- Increment counter for every execution unit completed
	signal execState : EXECUTION_STATE := EXEC_STATE_READY;
	signal executeDone : STD_LOGIC := '0';
	constant maxCycles : integer := 5;
	signal clockCycles : integer := 0;
	
begin

	DisplayString : DecodeDisplayString port map
	(
		reset 		=> reset,
		exec_state	=> execState,
		var_index 	=> var_index,	
		char_array 	=> char_array 	
	);

	EmitMsg : EmitLCD port map
	(
		clock 		=> clock, 
		reset 		=> reset, 	
		char_array 	=> char_array,	
		LCDDataBus 	=> LCD_DB, 
		LCD_E  		=> LCD_E,   
		LCD_RS 		=> LCD_RS,  
		LCD_RW 		=> LCD_RW
	);

	-------------------------------------
	-- Execution unit instances
	-------------------------------------
	Add0 : ALU_Add port map
	(
		reset => reset,
		op1 => a,
		op2 => b,
		result => c
	);
	
	
	-------------------------------------
	-- Entry point
	-- Triggers the program execution and sets the input args
	-------------------------------------
	DecodeArgs : process(execState)
	begin
		ResetSync : if reset = '0' then
			if execState = EXEC_STATE_RUNNING then
				GetInputSet : if input_set = "000" then
					x <= X"00000001";
					y <= X"00000002";
				elsif input_set = "001" then
					x <= X"00000003";
					y <= X"00000004";
				elsif input_set = "010" then
					x <= X"00000005";
					y <= X"00000006";
				elsif input_set = "011" then
					x <= X"00000007";
					y <= X"00000008";
				else
					x <= X"00000009";
					y <= X"0000000A";
				end if GetInputSet;
			end if;
		end if ResetSync;
	end process DecodeArgs;
	
	
	addop : process(x, y)
	begin
		ResetSync : if reset = '0' then
			a <= x;
			b <= y;
		end if ResetSync;
	end process addop;
	
	
	-------------------------------------
	-- Execution processes
	-------------------------------------
	assign_a : process(a)
	begin
		ResetSync : if reset = '0' then
			a_out <= a;
			
			----------------------------------------
			-- Temp LCD 
			-- LCD display string for this variable
			-- Buffer this string into the preview table
			----------------------------------------
			
		end if ResetSync;
	end process assign_a;
	
	assign_b : process(b)
	begin
		ResetSync : if reset = '0' then
			b_out <= b;
		end if ResetSync;
	end process assign_b;
	
	assign_c : process(c)
	begin
		ResetSync : if reset = '0' then
			c_out <= c;
		end if ResetSync;
	end process assign_c;
	
	
	-- This process sets the input from sw3-sw1 and 
	-- demultiplexes them to the input args or output preview
	SetInputArgsBits : process(reset, sw1, sw2, sw3)
	begin
		ResetSync : if reset = '1' then
			input_set <= "000";
		elsif reset = '0' then
			if execState = EXEC_STATE_READY then
				input_set(2) <= sw3;
				input_set(1) <= sw2;
				input_set(0) <= sw1;
			elsif execState = EXEC_STATE_DONE then
				var_index(2) <= sw3;
				var_index(1) <= sw2;
				var_index(0) <= sw1;
			end if;
		end if ResetSync;
	end process SetInputArgsBits;
	
	-- Determine if the program has ended by taking the number of clock cycles elapsed
	-- and comparing it to the estimated number of clock cycles the program should take to execute
	SetExecutionState : process(clock)
		variable cnt : integer;
		variable clkcyc : integer;
	begin
		ResetSync : if reset = '1' then
			clockCycles <= 0;
			executeDone <= '0';
		elsif reset = '0' then
			if execState = EXEC_STATE_RUNNING then 
				clockCycles <= clockCycles + 1;
				if clockCycles = maxCycles then
					executeDone <= '1';
				end if;
			end if;
		end if ResetSync;
	end process SetExecutionState;
	
	ExecutionStateTransition : process(reset, executeDone)
	begin
		ResetSync : if reset = '1' then
			execState <= EXEC_STATE_READY;
		elsif reset = '0' then
			if execState = EXEC_STATE_READY then
				execState <= EXEC_STATE_RUNNING;
			elsif execState = EXEC_STATE_RUNNING then
				if executeDone = '1' then
					execState <= EXEC_STATE_DONE;
				end if;
			elsif execState = EXEC_STATE_DONE then
			end if;
		end if ResetSync;
	end process ExecutionStateTransition;

end Behavioral;

